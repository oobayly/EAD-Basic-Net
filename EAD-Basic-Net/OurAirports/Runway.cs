using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents an OurAirports runway record.
  /// </summary>
  [Table("runways")]
  [Uri("http://ourairports.com/data/runways.csv")]
  public class Runway {
    #region Properties
    /// <summary>
    /// Gets or sets a flag indicating whether the runway is closed or not.
    /// </summary>
    [Column("closed")]
    [Required]
    public bool Closed { get; set; }

    /// <summary>
    /// Gets the internal ID of the runway.
    /// </summary>
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets the Ident of the airport to which the runway belongs.
    /// </summary>
    [Column("airport_ident")]
    [Index]
    [Required]
    public string Ident { get; set; }

    /// <summary>
    /// Gets or sets the length of the runway (ft).
    /// </summary>
    [Column("length_ft")]
    public int? Length { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating whether the runway is lit.
    /// </summary>
    [Column("lighted")]
    [Required]
    public bool Lighted { get; set; }

    /// <summary>
    /// Gets or sets the width of the runway (ft).
    /// </summary>
    [Column("width_ft")]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the type of surface of the runway.
    /// </summary>
    [Column("surface")]
    public string Surface { get; set; }
    #endregion

    #region Low end
    /// <summary>
    /// Gets or sets the elevation of the runway's low numbered end (ft).
    /// </summary>
    [Column("le_elevation_ft")]
    public int? LowElevation { get; set; }

    /// <summary>
    /// Gets or sets the heading of the runway's low numbered end (deg).
    /// </summary>
    [Column("le_heading_degT")]
    public decimal? LowHeading { get; set; }

    /// <summary>
    /// Gets or sets the Ident of the runway's low numbered end.
    /// </summary>
    [Column("le_ident")]
    public string LowIdent { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the runway's low numbered end.
    /// </summary>
    [Column("le_latitude_deg")]
    public float? LowLat { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the runway's low numbered end.
    /// </summary>
    [Column("le_longitude_deg")]
    public float? LowLon { get; set; }
    #endregion

    #region High end
    /// <summary>
    /// Gets or sets the elevation of the runway's high numbered end (ft).
    /// </summary>
    [Column("he_elevation_ft")]
    public int? HighElevation { get; set; }

    /// <summary>
    /// Gets or sets the heading of the runway's high numbered end (deg).
    /// </summary>
    [Column("he_heading_degT")]
    public decimal? HighHeading { get; set; }

    /// <summary>
    /// Gets or sets the Ident of the runway's high numbered end.
    /// </summary>
    [Column("he_ident")]
    public string HighIdent { get; set; }

    /// <summary>
    /// Gets or sets the latitude of the runway's high numbered end.
    /// </summary>
    [Column("he_latitude_deg")]
    public float? HighLat { get; set; }

    /// <summary>
    /// Gets or sets the longitude of the runway's high numbered end.
    /// </summary>
    [Column("he_longitude_deg")]
    public float? HighLon { get; set; }
    #endregion
  }
}
