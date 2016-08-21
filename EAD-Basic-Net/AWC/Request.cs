using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the request type of an AWC response.
  /// </summary>
  [XmlRoot("request")]
  public class Request {
    /// <summary>
    /// Gets or ses the type of request.
    /// </summary>
    [XmlAttribute("type")]
    public string Type { get; set; }
  }
}
