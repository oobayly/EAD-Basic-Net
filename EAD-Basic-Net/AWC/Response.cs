using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents a generic AWC report response.
  /// </summary>
  public abstract class Response<T> {
    /// <summary>
    /// Gets or sets the list of observation.
    /// </summary>
    [XmlArray("data")]
    public T[] Data { get; set; }
  }

  /// <summary>
  /// Represents a AWC METAR report response.
  /// </summary>
  [XmlRoot("response")]
  public class METARResponse : Response<METAR> {
  }
}