using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the forecast section of a TAF bulletin.
  /// </summary>
  [XmlRoot("forecast")]
  public class Forecast {
    #region Fields
    private string wx;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the start of the forecast time.
    /// </summary>
    [XmlElement("fcst_time_from")]
    public DateTime? TimeFrom { get; set; }

    /// <summary>
    /// Gets or sets the end time of the forecast.
    /// </summary>
    [XmlElement("fcst_time_to")]
    public DateTime? TimeTo { get; set; }

    /// <summary>
    /// Gets or sets the forecast change indicator.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [XmlElement("change_indicator")]
    public ChangeIndicator? ChangeIndicator { get; set; }

    /// <summary>
    /// Gets or sets the time becoming.
    /// </summary>
    [XmlElement("time_becoming")]
    public string TimeBecoming { get; set; }

    /// <summary>
    /// Gets or sets the probability of the forecase (%).
    /// </summary>
    [XmlElement("probability")]
    public int? Probability { get; set; }

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
    /// Gets or sets the wind gust height (Above ground level in Feet).
    /// </summary>
    [XmlElement("wind_shear_hgt_ft_agl")]
    public int? WindShearHeight { get; set; }

    /// <summary>
    /// Gets or sets the wind shear direction.
    /// </summary>
    [XmlElement("wind_shear_dir_degrees")]
    public int? WindSearDirection { get; set; }

    /// <summary>
    /// Gets or sets the wind shear speed (Knots).
    /// </summary>
    [XmlElement("wind_shear_speed_kt")]
    public int? WindShearSpeed { get; set; }

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
    /// Gets or sets the vertical visibility (Feet).
    /// </summary>
    [XmlElement("vert_vis_ft")]
    public int? VericalVisibility { get; set; }

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
    /// Gets or sets any text that hasn't been decoded.
    /// </summary>
    [XmlElement("not_decoded")]
    public string NotDecoded { get; set; }

    /// <summary>
    /// Gets or sets the sky condition.
    /// </summary>
    [XmlElement("sky_condition")]
    public SkyCondition[] SkyCondition { get; set; }

    /// <summary>
    /// Gets or sets the turbulence condition.
    /// </summary>
    [XmlElement("turbulence_condition")]
    public TurbulenceCondition[] Turbulence { get; set; }

    /// <summary>
    /// Gets or sets the turbulence condition.
    /// </summary>
    [XmlElement("icing_condition")]
    public IcingCondition[] Icing { get; set; }

    /// <summary>
    /// Gets or sets the temperatures.
    /// </summary>
    [XmlElement("temperature")]
    public Temperature[] Temperature { get; set; }
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
