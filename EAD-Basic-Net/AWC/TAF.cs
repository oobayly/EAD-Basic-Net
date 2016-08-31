using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents an AWC TAF bulletin.
  /// </summary>
  [XmlRoot("TAF")]
  public class TAF {
    /// <summary>
    /// Gets or sets the RAW TAF text.
    /// </summary>
    [XmlElement("raw_text")]
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the ID of the station.
    /// </summary>
    [XmlElement("station_id")]
    public string StationID { get; set; }

    /// <summary>
    /// Gets or sets the issue time (date and time the forecast was prepared).
    /// </summary>
    [XmlElement("issue_time")]
    public DateTime Issued { get; set; }

    /// <summary>
    /// Gets or sets the bulletin time.
    /// </summary>
    [XmlElement("bulletin_time")]
    public DateTime? BulletinTime { get; set; }

    /// <summary>
    /// Gets or sets the start time of when the report is valid.
    /// </summary>
    [XmlElement("valid_time_from")]
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// Gets or sets the end time for when the report is valid.
    /// </summary>
    [XmlElement("valid_time_to")]
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// Gets or sets any remarks for the bulletin.
    /// </summary>
    [XmlElement("remarks")]
    public string Remarks { get; set; }

    /// <summary>
    /// Gets or sets the observation time.
    /// </summary>
    [XmlElement("observation_time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the station.
    /// </summary>
    [XmlElement("latitude")]
    public float? Lat { get; set; }

    /// <summary>
    /// Gets or sets the station elevation (Metres).
    /// </summary>
    [XmlElement("elevation_m")]
    public float? Elevation { get; set; }

    /// <summary>
    /// Gets or sets the forecasts.
    /// </summary>
    [XmlElement("forecast")]
    public Forecast[] Forecast { get; set; }
  }
}
