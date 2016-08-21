using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents an warning that can be returned by the AWC website.
  /// </summary>
  [XmlRoot("warnings")]
  public class Warning {
    /// <summary>
    /// Gets or sets the text description of the warnings.
    /// </summary>
    [XmlAttribute("warning")]
    public string Text { get; set; }
  }
}
