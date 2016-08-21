using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the turbulence condition value for a TAF report.
  /// </summary>
  [XmlRoot("turbulence_condition")]
  public class TurbulenceCondition {
    /// <summary>
    /// Gets or sets the turbulence intensity.
    /// </summary>
    [XmlAttribute("turbulence_intensity")]
    public string Intensity { get; set; }

    /// <summary>
    /// Gets or sets the minimum turbulence altitude (Above ground level in Feet).
    /// </summary>
    [XmlAttribute("turbulence_min_alt_ft_agl")]
    public int MinAlt { get; set; }

    /// <summary>
    /// Gets or sets the maximum turbulence altitude (Above ground level in Feet).
    /// </summary>
    [XmlElement("turbulence_max_alt_ft_agl")]
    public int MaxAlt { get; set; }
  }
}
