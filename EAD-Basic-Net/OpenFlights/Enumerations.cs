using System;
using System.ComponentModel;

namespace eu.bayly.EADBasicNet.OpenFlights {
  /// <summary>
  /// The daylight savings region.
  /// </summary>
  public enum DST {
    /// <summary>Europe</summary>
    [Description("Europe")]
    E,
    /// <summary>USA/Canada</summary>
    [Description("USA/Canada")]
    A,
    /// <summary>South America</summary>
    [Description("South America")]
    S,
    /// <summary>Australia</summary>
    [Description("Australia")]
    O,
    /// <summary>New Zealand</summary>
    [Description("New Zealand")]
    Z,
    /// <summary>None</summary>
    [Description("None")]
    N,
    /// <summary>Unknown</summary>
    [Description("Unknown")]
    U
  }
}