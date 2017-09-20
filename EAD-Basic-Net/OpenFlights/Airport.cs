using Csv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;

namespace eu.bayly.EADBasicNet.OpenFlights {
  /// <summary>
  /// Represents an OpenFlights airport record.
  /// </summary>
  public class Airport {
    #region Constants
    /// <summary>
    /// The location of the OpenFlights airports database.
    /// </summary>
    public const string DataUri = "https://raw.githubusercontent.com/jpatokal/openflights/master/data/airports.dat";
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the altitude of the airport (Feet).
    /// </summary>
    public int Alt { get; set; }

    /// <summary>
    /// Gets or sets the city of the airport.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Gets or sets the name of the country where the airport is located.
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Gets or sets the daylight savings region.
    /// </summary>
    [DefaultValue("U")]
    public DST DST { get; set; }

    /// <summary>
    /// Gets or sets the 3-character IATA/FAA airport code.
    /// </summary>
    public string IATA { get; set; }

    /// <summary>
    /// Gets or sets the 4-character ICAO airport code.
    /// </summary>
    public string ICAO { get; set; }

    /// <summary>
    /// Gets or sets the ID of the airport.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the airport.
    /// </summary>
    public double Lat { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the airport.
    /// </summary>
    public double Lon { get; set; }

    /// <summary>
    /// Gets or sets the name of the airport.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the timezone (Olson).
    /// </summary>
    /// <remarks>See https://en.wikipedia.org/wiki/Tz_database </remarks>
    public string TZ { get; set; }

    /// <summary>
    /// Gets or sets the timezone offset for the airport.
    /// </summary>
    public float? UTCOffset { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates an instance of the Airport class.
    /// </summary>
    public Airport() {
      DST = DST.U;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Creates a list of airports from the specified stream.
    /// </summary>
    /// <param name="stream">The stream from which the airports are read.</param>
    /// <param name="match">An optional set of criteria that the airports should match</param>
    public static Airport[] FromStream(Stream stream, Predicate<Airport> match = null) {
      var options = new CsvOptions() {
        HeaderMode = HeaderMode.HeaderAbsent,
        Separator = ',',
        TrimData = true,
        ValidateColumnCount = true
      };

      var list = new List<Airport>();

      foreach (var line in CsvReader.ReadFromStream(stream, options)) {
        var item = new Airport() {
          ID = int.Parse(line[0]),
          Name = line[1],
          City = line[2],
          Country = line[3],
          //IATA = line[4],
          //ICAO = line[5],
          Lat = double.Parse(line[6]),
          Lon = double.Parse(line[7]),
          Alt = int.Parse(line[8]),
          //UTCOffset = float.Parse(line[9]),
          //DST,
          TZ = line[11] == "\\N" ? null : line[11],
        };

        if (!string.IsNullOrEmpty(line[4]) && (line[4] != "\\N"))
          item.IATA = line[4];

        if (!string.IsNullOrEmpty(line[5]))
          item.ICAO = line[5];

        float offset;
        if (float.TryParse(line[9], out offset)) {
          item.UTCOffset = offset;
        }

        DST dst;
        if (!Enum.TryParse<DST>(line[10], out dst)) {
          item.DST = dst;
        } else {
          item.DST = DST.U;
        }

        if ((match == null) || match.Invoke(item))
          list.Add(item);
      }

      return list.ToArray();
    }

    /// <summary>
    /// Creates a list of airports from the OpenFlights airports.dat file.
    /// </summary>
    /// <param name="localFile">An optional file used for caching.</param>
    /// <param name="match">An optional set of criteria that the airports should match</param>
    public static Airport[] FromOpenFlights(FileInfo localFile = null, Predicate<Airport> match = null) {
      var req = (HttpWebRequest)HttpWebRequest.Create(DataUri);
      if ((localFile != null) & localFile.Exists)
        req.IfModifiedSince = localFile.LastWriteTime;

      try {
        using (var resp = (HttpWebResponse)req.GetResponse()) {
          using (var stream = resp.GetResponseStream()) {
            if (localFile == null) {
              return FromStream(stream, match);

            } else {
              Airport[] list;
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