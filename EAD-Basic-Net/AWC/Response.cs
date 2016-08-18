using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents a generic AWC report response.
  /// </summary>
  [XmlRoot("response")]
  public abstract class Response<T> {
    #region Constants
    /// <summary>
    /// The Uri which contains the observation data.
    /// </summary>
    public const string ReportUri = "https://aviationweather.gov/adds/dataserver_current/httpparam?";
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the list of observation.
    /// </summary>
    [XmlArray("data")]
    public T[] Data { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// Fetches a report from the the specified stream.
    /// </summary>
    public static Response<T> FromStream(Stream stream) {
      Type t;
      if (typeof(T) == typeof(METAR)) {
        t = typeof(METARResponse);
      } else {
        throw new Exception("Invalid generic type.");
      }

      var sz = new XmlSerializer(t);
      return (Response<T>)sz.Deserialize(stream);
    }

    /// <summary>
    /// Fetches a report from the NOAA's AWS website.
    /// </summary>
    public static Response<T> FromAWS(string icao) {
      string reportType;
      if (typeof(T) == typeof(METAR)) {
        reportType = "metars";
      } else {
        throw new Exception("Invalid generic type.");
      }

      var uri = ReportUri +
        "dataSource=" + reportType +
        "&requestType=retrieve&format=xml" +
        string.Format("&hoursBeforeNow={0}", 1) +
        string.Format("&mostRecent={0}", false) +
        "&stationString=" + icao;

      var req = (HttpWebRequest)HttpWebRequest.Create(uri);

      using (var resp = (HttpWebResponse)req.GetResponse()) {
        using (var stream = resp.GetResponseStream()) {
          return FromStream(stream);
        }
      }
    }
    #endregion
  }

  /// <summary>
  /// Represents a AWC METAR report response.
  /// </summary>
  [XmlRoot("response")]
  public class METARResponse : Response<METAR> {
  }
}