using System;
using System.ComponentModel;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// The change indicator for a TAF bulletin forecast.
  /// </summary>
  public enum ChangeIndicator {
    /// <summary>Invalid value.</summary>
    Invalid,

    /// <summary>emporary fluctuation in some of the elements lasting for periods of 30 minutes or more but not longer than one hour</summary>
    [Description("Temporary")]
    TEMPO,
    /// <summary>Used to indicate a gradual change in some of the forecast elements.</summary>
    [Description("Becoming")]
    BECMG,
    /// <summary>Used when a significant change in all elements is expected at a specific time.</summary>
    [Description("From")]
    FM,
    /// <summary>The probability or chance of thunderstorms or other precipitation events occuring.</summary>
    [Description("Probability")]
    PROB
  }

  /// <summary>
  /// The type of cloud covers used in METAR/TAF observations.
  /// </summary>
  public enum CloudCover {
    /// <summary>Invalid value.</summary>
    Invalid,

    /// <summary>Ceiling and visibility okay.</summary>
    [Description("Ceiling and visibility okay")]
    CAVOK,

    /// <summary>No cloud / Sky clear.</summary>
    [Description("Sky clear")]
    SKC,
    /// <summary>No clouds below 12,000 ft.</summary>
    [Description("No clouds below 12,000 ft")]
    CLR,
    /// <summary>No significant cloud.</summary>
    [Description("No significant cloud")]
    NSC,
    /// <summary>Few: 1-2 oktas</summary>
    [Description("Few")]
    FEW,
    /// <summary>Scattered: 3-4 oktas.</summary>
    [Description("Scattered")]
    SCT,
    /// <summary>Broken: 5-7 oktas.</summary>
    [Description("Broken")]
    BKN,
    /// <summary>Overcast: 8 oktas.</summary>
    [Description("Overcast")]
    OVC,
    /// <summary>Clouds cannot be seen.</summary>
    [Description("Clouds cannot be seen")]
    VV
  }

  /// <summary>
  /// The type of METAR observation.
  /// </summary>
  public enum METARType {
    /// <summary>Regular METAR observation.</summary>
    METAR,
    /// <summary>Special weather observation.</summary>
    SPECI
  }

  /// <summary>
  /// The type of most recent response data to filter by.
  /// </summary>
  public enum MostRecentType {
    /// <summary>Don't apply filtering.</summary>
    Off,
    /// <summary>Show the most recent observation or report of any station.</summary>
    [Description("Any station")]
    Any,
    /// <summary>Show the most recent observation or report for each station.</summary>
    [Description("Each station")]
    Each
  }

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