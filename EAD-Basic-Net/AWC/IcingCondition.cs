using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the icing condition value for a TAF report.
  /// </summary>
  [XmlRoot("icing_condition")]
  public class IcingCondition {
    /// <summary>
    /// Gets or sets the icing intensity.
    /// </summary>
    [XmlAttribute("turbulence_intensity")]
    public string Intensity { get; set; }

    /// <summary>
    /// Gets or sets the minimum icing altitude (Above ground level in Feet).
    /// </summary>
    [XmlAttribute("icing_min_alt_ft_agl")]
    public int MinAlt { get; set; }

    /// <summary>
    /// Gets or sets the maximum icing altitude (Above ground level in Feet).
    /// </summary>
    [XmlElement("icing_max_alt_ft_agl")]
    public int MaxAlt { get; set; }
  }
}
