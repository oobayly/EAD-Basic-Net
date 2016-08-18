using System;
using System.ComponentModel;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Attributes that a weather station may have.
  /// </summary>
  [Flags]
  public enum StationAttributes {
    /// <summary>
    /// No attributes.
    /// </summary>
    None = 0x0,

    /// <summary>METAR Station</summary>
    METAR = 0x1,
    /// <summary>Next-Generation Radar</summary>
    [Description("Next-Generation Radar")]
    NEXRAD = 0x2,

    // Aviation specific flags
    /// <summary>Airmen's Meteorological Information</summary>
    [Description("Airmen's Meteorological Information")]
    AIRMET = 0x4,
    /// <summary>Air Route Traffic Control Centers</summary>
    [Description("Air Route Traffic Control Center")]
    ARTCC = 0x8,
    /// <summary>Terminal aerodrome forecast</summary>
    [Description("Terminal aerodrome forecast")]
    TAF = 0x10,

    // Upper air
    /// <summary>Radar-Wind Sonde</summary>
    [Description("Radar-Wind Sonde")]
    Rawinsonde = 0x20,
    /// <summary>Wind Profile</summary>
    [Description("Wind Profile")]
    WindProfile = 0x40,

    // Automatics
    /// <summary>Automated Surface Observing System</summary>
    [Description("Automated Surface Observing System")]
    ASOS = 0x80,
    /// <summary>Automated Weather Observing System</summary>
    [Description("Automated Weather Observing System")]
    AWOS = 0x100,
    /// <summary>Mesonet</summary>
    [Description("Mesonet")]
    Meso = 0x200,
    /// <summary>Human</summary>
    Human = 0x400,
    /// <summary>Augmented</summary>
    Augmented = 0x800,

    // Office type
    /// <summary>Weather Forecast Office</summary>
    [Description("Weather Forecast Office")]
    WFO = 0x1000,
    /// <summary>River Forecast Centers</summary>
    [Description("River Forecast Centers")]
    RFC = 0x2000,
    /// <summary>National Centers for Environmental Prediction</summary>
    [Description("National Centers for Environmental Prediction")]
    NCEP = 0x4000,
  }
}