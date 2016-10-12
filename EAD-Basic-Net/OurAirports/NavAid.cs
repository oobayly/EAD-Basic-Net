using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  //"A foreign key value cannot be inserted because a corresponding primary key value does not exist. [ Foreign key constraint name = FK_dbo.navaids_dbo.airports_ident ]"
  [Table("navaids")]
  [Uri("http://ourairports.com/data/navaids.csv")]
  public class NavAid {
    #region Enumerations
    public enum NavAidPowers {
      Unknown,
      Low,
      Medium,
      High
    }

    public enum NavAidTypes {
      /// <summary>Distance measuring equipment</summary>
      DME,
      /// <summary>Non-directional beacon</summary>
      NDB,
      /// <summary>Non-directional beacon &amp; distance measuring equipment</summary>
      [Description("NDB-DME")]
      NDBDME,
      /// <summary>Tactical air navigation</summary>
      TACAN,
      /// <summary>VHF omni-directional radio range</summary>
      VOR,
      /// <summary>VHF omni-directional radio range &amp; distance measuring equipment</summary>
      [Description("VOR-DME")]
      VORDME,
      /// <summary>VHF omni-directional radio range &amp; tactical air navigation</summary>
      [Description("")]
      VORTAC
    }

    public enum NavAidUsages {
      Unknown,
      Lo,
      Hi,
      Both,
      RNAV,
      Terminal
    }
    #endregion

    #region Properties
    [ForeignKey("AirportIdent")]
    public Airport Airport { get; set; }

    [Column("associated_airport")]
    [Index]
    public string AirportIdent { get; set; }

    [ForeignKey("CountryCode")]
    public Country Country { get; set; }

    [Column("iso_country")]
    [Index]
    [MaxLength(2)]
    public string CountryCode { get; set;}

    [Column("dme_frequency_khz")]
    public int? DMEFreq { get; set; }

    [Column("dme_channel")]
    public string DMEChannel { get; set; }

    [Column("dme_elevation_ft")]
    public int? DMEElevation { get; set; }

    [Column("dme_latitude_deg")]
    public float? DMELat { get; set; }

    [Column("dme_longitude_deg")]
    public float? DMELon { get; set; }

    [Column("elevation_ft")]
    public int? Elevation { get; set; }

    [Column("frequency_khz")]
    [Required]
    public int Freq { get; set; }

    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public int ID { get; set; }

    [Column("ident")]
    [Index]
    [MaxLength(8)] // Some muppet added NDB to the Ident (Wau_NDB)
    [Required]
    public string Ident { get; set; }

    [Column("latitude_deg")]
    [Required]
    public float Lat { get; set; }

    [Column("longitude_deg")]
    [Required]
    public float Lon { get; set; }

    [Column("magnetic_variation_deg")]
    public decimal? MagneticVariation { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("power")]
    public NavAidPowers? Power { get; set; }

    [Column("slaved_variation_deg")]
    public decimal? SlavedVariation { get; set; }

    [Column("type")]
    [Required]
    public NavAidTypes Type { get; set; }

    [Column("usageType")]
    public NavAidUsages? UsageType { get; set; }
    #endregion
  }
}
