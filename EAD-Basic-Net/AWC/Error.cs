using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents an error that can be returned by the AWC website.
  /// </summary>
  [XmlRoot("errors")]
  public class Error {
    /// <summary>
    /// Gets or sets the text description of the error.
    /// </summary>
    [XmlAttribute("error")]
    public string Text { get; set; }
  }
}
