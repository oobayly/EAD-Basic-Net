using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the temperature value for a TAF report.
  /// </summary>
  [XmlRoot("temperature")]
  public class Temperature {
    /// <summary>
    /// Gets or sets the time.
    /// </summary>
    [XmlAttribute("valid_time")]
    public string Time { get; set; }

    /// <summary>
    /// Gets or sets the surface temperature (Celsius).
    /// </summary>
    [XmlAttribute("sfc_temp_c")]
    public float SurfaceTemp { get; set; }

    /// <summary>
    /// Gets or sets the maximum temperature (Celsius).
    /// </summary>
    [XmlElement("max_temp_c")]
    public float MaxTemp { get; set; }

    /// <summary>
    /// Gets or sets the minimum temperature (Celsius).
    /// </summary>
    [XmlElement("min_temp_c")]
    public float MinTemp { get; set; }
  }
}
