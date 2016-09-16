using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents an AWC METAR observation.
  /// </summary>
  [XmlRoot("METAR")]
  public class METAR {
    #region Fields
    private string wx;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the RAW METAR text.
    /// </summary>
    [XmlElement("raw_text")]
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the ID of the station.
    /// </summary>
    [XmlElement("station_id")]
    public string StationID { get; set; }

    /// <summary>
    /// Gets or sets the observation time.
    /// </summary>
    [XmlElement("observation_time")]
    public DateTime Time { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the station.
    /// </summary>
    [XmlElement("latitude")]
    public float? Lat { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the station.
    /// </summary>
    [XmlElement("longitude")]
    public float? Lon { get; set; }

    /// <summary>
    /// Gets or sets the temperature (Celsius)
    /// </summary>
    [XmlElement("temp_c")]
    public float? Temp { get; set; }

    /// <summary>
    /// Gets or sets the dew point? (Celsius)
    /// </summary>
    [XmlElement("dewpoint_c")]
    public float? DewPoint { get; set; }

    /// <summary>
    /// Gets or sets the wind direction.
    /// </summary>
    [XmlElement("wind_dir_degrees")]
    public int? WindDirection { get; set; }

    /// <summary>
    /// Gets or sets the wind speed (Knots).
    /// </summary>
    [XmlElement("wind_speed_kt")]
    public int? WindSpeed { get; set; }

    /// <summary>
    /// Gets or sets the wind gust speed (Knots).
    /// </summary>
    [XmlElement("wind_gust_kt")]
    public int? WindGust { get; set; }

    /// <summary>
    /// Gets or sets the visibility (Statute miles).
    /// </summary>
    [XmlElement("visibility_statute_mi")]
    public float? Visibility { get; set; }

    /// <summary>
    /// Gets or sets the altimeter reading (Inches of mercury).
    /// </summary>
    [XmlElement("altim_in_hg")]
    public float? Altimeter { get; set; }

    /// <summary>
    /// Gets or sets the sea-level pressure (Millibars)
    /// </summary>
    [XmlElement("sea_level_pressure_mb")]
    public float? SeaLevelPressure { get; set; }

    /// <summary>
    /// Gets or sets the quality-control flags.
    /// </summary>
    [XmlElement("quality_control_flags")]
    public QualityControlFlags QualityControlFlags { get; set; }

    /// <summary>
    /// Gets or sets the WX text.
    /// </summary>
    [XmlElement("wx_string")]
    public string WXText {
      get {
        return wx;
      }
      set {
        wx = value;
        OnWXChanged();
      }
    }

    /// <summary>
    /// Gets or sets the detailed WX description.
    /// </summary>
    [XmlIgnore]
    public string WXDescription { get; private set; }

    /// <summary>
    /// Gets or sets the sky condition.
    /// </summary>
    [XmlElement("sky_condition")]
    public SkyCondition[] SkyCondition { get; set; }

    /// <summary>
    /// Gets or sets the flight category.
    /// </summary>
    [XmlElement("flight_category")]
    public string FlightCategory { get; set; }

    /// <summary>
    /// Gets or sets the pressure change over the last 3 hours (Millibars).
    /// </summary>
    [XmlElement("three_hr_pressure_tendency_mb")]
    public float? ThreeHourPressureTendency { get; set; }

    /// <summary>
    /// Gets or sets the maximum temperature over the last 6 hours (Celsius).
    /// </summary>
    [XmlElement("maxT_c")]
    public float? MaxTemp { get; set; }

    /// <summary>
    /// Gets or sets the minimum temperature over the last 6 hours (Celsius).
    /// </summary>
    [XmlElement("minT_c")]
    public float? MinTemp { get; set; }

    /// <summary>
    /// Gets or sets the maximum temperature over the last 24 hours (Celsius).
    /// </summary>
    [XmlElement("maxT24hr_c")]
    public float? MaxTemp24hr { get; set; }

    /// <summary>
    /// Gets or sets the minimum temperature over the last 24 hours (Celsius).
    /// </summary>
    [XmlElement("minT24hr_c")]
    public float? MinTemp24hr { get; set; }

    /// <summary>
    /// Gets or sets the liquid precipitaion since the last regular METAR observation (Inches).
    /// </summary>
    [XmlElement("precip_in")]
    public float? Precip { get; set; }

    /// <summary>
    /// Gets or sets the liquid precipitation in the last 3 hours (Inches).
    /// </summary>
    [XmlElement("pcp3hr_in")]
    public float? Precip3hr { get; set; }

    /// <summary>
    /// Gets or sets the liquid precipitation in the last 6 hours (Inches).
    /// </summary>
    [XmlElement("pcp6hr_in")]
    public float? Precip6hr { get; set; }

    /// <summary>
    /// Gets or sets the liquid precipitation in the last 24 hours (Inches).
    /// </summary>
    [XmlElement("pcp24hr_in")]
    public float? Precip24hr { get; set; }

    /// <summary>
    /// Gets or sets the snow depth on the ground (Inches).
    /// </summary>
    [XmlElement("snow_in")]
    public float? Snow { get; set; }

    /// <summary>
    /// Gets or sets the vertical visibility (Feet).
    /// </summary>
    [XmlElement("vert_vis_ft")]
    public int? VericalVisibility { get; set; }

    /// <summary>
    /// Gets or sets the METAR type.
    /// </summary>
    [XmlElement("metar_type")]
    public METARType METARType { get; set; }

    /// <summary>
    /// Gets or sets the station elevation (Metres).
    /// </summary>
    [XmlElement("elevation_m")]
    public float? Elevation { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// Raised when the WX property is changed.
    /// </summary>
    protected void OnWXChanged() {
      // TODO: Implement this.
    }
    #endregion
  }
}
