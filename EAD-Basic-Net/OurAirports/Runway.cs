using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  [Table("runways")]
  [Uri("http://ourairports.com/data/runways.csv")]
  public class Runway {
    #region Properties
    [ForeignKey("Ident")]
    public Airport Airport { get; set; }

    [Column("closed")]
    [Required]
    public bool Closed { get; set; }

    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public int ID { get; set; }

    [Column("airport_ident")]
    [Index]
    [Required]
    public string Ident { get; set; }

    [Column("length_ft")]
    public int? Length { get; set; }

    [Column("lighted")]
    [Required]
    public bool Lighted { get; set; }

    [Column("width_ft")]
    public int? Width { get; set; }

    [Column("surface")]
    public string Surface { get; set; }
    #endregion

    #region Low end
    [Column("le_elevation_ft")]
    public int? LowElevation { get; set; }

    [Column("le_heading_degT")]
    public decimal? LowHeading { get; set; }

    [Column("le_ident")]
    public string LowIdent { get; set; }

    [Column("le_latitude_deg")]
    public float? LowLat { get; set; }

    [Column("le_longitude_deg")]
    public float? LowLon { get; set; }
    #endregion

    #region High end
    [Column("he_elevation_ft")]
    public int? HighElevation { get; set; }

    [Column("he_heading_degT")]
    public decimal? HighHeading { get; set; }

    [Column("he_ident")]
    public string HighIdent { get; set; }

    [Column("he_latitude_deg")]
    public float? HighLat { get; set; }

    [Column("he_longitude_deg")]
    public float? HighLon { get; set; }
    #endregion
  }
}
