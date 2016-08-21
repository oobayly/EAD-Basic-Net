using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents an NOAA weather station.
  /// </summary>
  public class Station {
    #region Constants
    /// <summary>
    /// The Uri which contains the station data.
    /// </summary>
    private const string StationUri = "https://aviationweather.gov/docs/metar/stations.txt";
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the altitude of the station (in metres).
    /// </summary>
    public int Alt { get; set; }

    /// <summary>
    /// Gets or sets the station's attributes.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public StationAttributes Attributes { get; set; }

    /// <summary>
    /// Gets or sets the code of the country in which the station is located.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the station's 3-character International Air Transport Association (IATA) code.
    /// </summary>
    public string IATA { get; set; }

    /// <summary>
    /// Gets or sets the station's 4-character International Civil Aviation Organization (ICAO) code.
    /// </summary>
    public string ICAO { get; set; }

    /// <summary>
    /// Gets or sets the station's latitude.
    /// </summary>
    public float Lat { get; set; }

    /// <summary>
    /// Gets or sets the station's longitude.
    /// </summary>
    public float Lon { get; set; }

    /// <summary>
    /// Gets or sets the name of the station.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the priority when plotting (lower = highest).
    /// </summary>
    public byte Priority { get; set; }

    /// <summary>
    /// Gets or sets the 2-letter state (province in Canada abbreviation.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Gets or sets the 5-digit internation synoptic number.
    /// </summary>
    public int? Synoptic { get; set; }
    #endregion

    #region Constructors
    #endregion

    #region Methods
    /// <summary>
    /// Creates a list of NOAA weather stations from the specified stream.
    /// </summary>
    /// <param name="stream">The stream from which the stations are read.</param>
    /// <param name="match">An optional set of criteria that the airports should match</param>
    public static Station[] FromStream(Stream stream, Predicate<Station> match = null) {
      var list = new List<Station>();

      using (var reader = new StreamReader(stream)) {
        string line;

        while ((line = reader.ReadLine()) != null) {
          // Comments begine with !
          if (line.StartsWith("!"))
            continue;

          // Records must be 83 chars
          if (line.Length != 83)
            continue;

          string text;
          var item = new Station() {
            State = line.StartsWith("  ") ? null : line.Substring(0, 2),
            Name = line.Substring(3, 16).Trim(),
            ICAO = line.Substring(20, 4),
            IATA = line.Substring(26, 3),
            //Synoptic
            Lat = (float.Parse(line.Substring(39, 2)) + (float.Parse(line.Substring(42, 2)) / 60)) * ((line.Substring(44, 1) == "N") ? 1 : -1),
            Lon = (float.Parse(line.Substring(47, 3)) + (float.Parse(line.Substring(51, 2)) / 60)) * ((line.Substring(52, 1) == "E") ? 1 : -1),
            Alt = int.Parse(line.Substring(55, 4)),
            // Attributes
            Priority = byte.Parse(line.Substring(79, 1)),
            Country = line.Substring(81, 2)
          };

          if (item.IATA == "   ")
            item.IATA = null;
          if (item.ICAO == "    ")
            item.ICAO = null;

          // Synoptic
          text = line.Substring(32, 5).Trim();
          if (text.Length != 0) {
            item.Synoptic = int.Parse(text);
          }

          var attr = StationAttributes.None;
          // METAR
          if (line.Substring(62, 1) == "X")
            attr |= StationAttributes.METAR;
          // NEXRAD
          if (line.Substring(65, 1) == "X")
            attr |= StationAttributes.NEXRAD;
          // Aviation specific
          switch (line[68]) {
            case 'V':
              attr |= StationAttributes.AIRMET;
              break;
            case 'A':
              attr |= StationAttributes.ARTCC;
              break;
            case 'T':
              attr |= StationAttributes.TAF;
              break;
            case 'U':
              attr |= StationAttributes.AIRMET | StationAttributes.TAF;
              break;
          }
          // Upper air
          switch (line[71]) {
            case 'X':
              attr |= StationAttributes.Rawinsonde;
              break;
            case 'W':
              attr |= StationAttributes.WindProfile;
              break;
          }
          // Automatics
          switch (line[74]) {
            case 'A':
              attr |= StationAttributes.ASOS;
              break;
            case 'W':
              attr |= StationAttributes.AWOS;
              break;
            case 'M':
              attr |= StationAttributes.Meso;
              break;
            case 'H':
              attr |= StationAttributes.Human;
              break;
            case 'G':
              attr |= StationAttributes.Augmented;
              break;
          }
          // Office type
          switch (line[77]) {
            case 'F':
              attr |= StationAttributes.WFO;
              break;
            case 'R':
              attr |= StationAttributes.RFC;
              break;
            case 'C':
              attr |= StationAttributes.NCEP;
              break;
          }
          item.Attributes = attr;

          if ((match == null) || match.Invoke(item))
            list.Add(item);
        }
      }

      return list.ToArray();
    }

    /// <summary>
    /// Creates a list of NOAA weather stations NOAA's AWS website.
    /// </summary>
    /// <param name="localFile">An optional file used for caching.</param>
    /// <param name="match">An optional set of criteria that the airports should match</param>
    public static Station[] FromAWS(FileInfo localFile = null, Predicate<Station> match = null) {
      var req = (HttpWebRequest)HttpWebRequest.Create(StationUri);
      if ((localFile != null) & localFile.Exists)
        req.IfModifiedSince = localFile.LastWriteTime;

      try {
        using (var resp = (HttpWebResponse)req.GetResponse()) {
          using (var stream = resp.GetResponseStream()) {
            if (localFile == null) {
              return FromStream(stream, match);

            } else {
              // Copy the stream into the local file & parse
              Station[] list;
              using (var fs = localFile.Open(FileMode.Create, FileAccess.ReadWrite)) {
                stream.CopyTo(fs);
                fs.Flush();
                fs.Position = 0;
                list = FromStream(fs, match);
              }

              // Use the last modified date.
              localFile.Refresh();
              if (resp.LastModified != DateTime.MinValue)
                localFile.LastWriteTime = resp.LastModified;

              return list;

            }
          }
        }

      } catch (WebException ex) {
        // NotModified is now a valid response.
        var resp = ex.Response as HttpWebResponse;
        if (resp.StatusCode == HttpStatusCode.NotModified) {
          using (var fs = localFile.OpenRead()) {
            return FromStream(fs, match);
          }

        } else {
          throw;
        }

      }
    }
    #endregion
  }
}