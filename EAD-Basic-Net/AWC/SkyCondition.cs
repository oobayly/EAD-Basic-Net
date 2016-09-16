﻿using eu.bayly.EADBasicNet;
using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the sky condition value for a METAR/TAF report.
  /// </summary>
  [XmlRoot("sky_condition")]
  public class SkyCondition {
    /// <summary>
    /// Gets or sets the type of cover.
    /// </summary>
    [XmlAttribute("sky_cover")]
    public CloudCover Cover { get; set; }

    /// <summary>
    /// Gets or sets the cloud base (Above ground level in Feet).
    /// </summary>
    [XmlAttribute("cloud_base_ft_agl")]
    public string Base { get; set; }

    /// <summary>
    /// Gets or sets the type of cloud.
    /// </summary>
    [XmlElement("cloud_type")]
    public string Type { get; set; }

    /// <summary>
    /// Gets the human readable text of the type of cover.
    /// </summary>
    [XmlIgnore]
    public string CoverName {
      get {
        return Cover.ToDescription();
      }
    }
  }
}
