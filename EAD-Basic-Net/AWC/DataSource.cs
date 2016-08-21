using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the datasource of an AWC response.
  /// </summary>
  [XmlRoot("data_source")]
  public class DataSource {
    /// <summary>
    /// Gets or setst the name of the datasource.
    /// </summary>
    [XmlAttribute("name")]
    public string Name { get; set; }
  }
}
