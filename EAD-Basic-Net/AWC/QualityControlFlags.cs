using System;
using System.Xml.Serialization;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents the qyality control flags value for a METAR report.
  /// </summary>
  [XmlRoot("quality_control_flags")]
  public class QualityControlFlags {
    /// <summary>
    /// Gets or sets whether the report was corrected/
    /// </summary>
    [XmlElement("corrected")]
    public string Corrected { get; set; }

    /// <summary>
    /// Gerts or sets if the report was fully automated.
    /// </summary>
    [XmlElement("auto")]
    public string Auto { get; set; }

    /// <summary>
    /// Gets or sets if the station is one of the following types:
    /// A01|A01A|A02|A02A|AOA|AWOS
    /// </summary>
    [XmlElement("auto_station")]
    public string AutoStation { get; set; }

    /// <summary>
    /// Gets orsets if maintenance is needed.
    /// </summary>
    [XmlElement("maintenance_indicator_on")]
    public string MaintenanceIndicatorOn { get; set; }

    /// <summary>
    /// Gets or sets if there is no signal.
    /// </summary>
    [XmlElement("no_signal")]
    public string NoSignal { get; set; }

    /// <summary>
    /// Gets or sets if the lightning detection sensor is not operating.
    /// </summary>
    [XmlElement("lightning_sensor_off")]
    public string LightningSensorOff { get; set; }

    /// <summary>
    /// Gets or sets if the freezing rain sensor is not operating.
    /// </summary>
    [XmlElement("freezing_rain_sensor_off")]
    public string FreezingRainSensorOff { get; set; }

    /// <summary>
    /// Gets or sets if the present weather sensor is not operating.
    /// </summary>
    [XmlElement("present_weather_sensor_off")]
    public string PresentWeatherSensorOff { get; set; }
  }
}