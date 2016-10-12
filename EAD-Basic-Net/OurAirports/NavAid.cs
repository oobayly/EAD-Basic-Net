using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents an OurAirports airport frequency record.
  /// </summary>
  [Table("navaids")]
  [Uri("http://ourairports.com/data/navaids.csv")]
  public class NavAid {
    #region Enumerations
    /// <summary>
    /// Represents the power of a NavAid.
    /// </summary>
    public enum NavAidPowers {
      /// <summary>Unknown</summary>
      Unknown,
      /// <summary>Low</summary>
      Low,
      /// <summary>Medium</summary>
      Medium,
      /// <summary>High</summary>
      High
    }

    /// <summary>
    /// Represents the type of NavAid.
    /// </summary>
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

    /// <summary>
    /// Represents the usage type of a NavAid.
    /// </summary>
    public enum NavAidUsages {
      /// <summary>Unknown</summary>
      Unknown,
      /// <summary>Low altitude.</summary>
      [Description("Low")]
      Lo,
      /// <summary>High altitude.</summary>
      [Description("High")]
      Hi,
      /// <summary>High &amp; Low altitude.</summary>
      [Description("High & Low")]
      Both,
      /// <summary>Area Navigation.</summary>
      RNAV,
      /// <summary>Terminal NavAid.</summary>
      Terminal
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the airport that the navaid belongs to.
    /// </summary>
    [ForeignKey("AirportIdent")]
    public Airport Airport { get; set; }

    /// <summary>
    /// Gets or sets the Ident of the airport that the navaid belongs to.
    /// </summary>
    [Column("associated_airport")]
    [Index]
    public string AirportIdent { get; set; }

    /// <summary>
    /// Gets or sets the 2-character ISO country code.
    /// </summary>
    [ForeignKey("CountryCode")]
    public Country Country { get; set; }

    /// <summary>
    /// Gets or sets the 2-character ISO country code.
    /// </summary>
    [Column("iso_country")]
    [Index]
    [MaxLength(2)]
    public string CountryCode { get; set;}

    /// <summary>
    /// Gets or sets the frequency of the DME (kHz).
    /// </summary>
    [Column("dme_frequency_khz")]
    public int? DMEFreq { get; set; }

    /// <summary>
    /// Gets or sets the DME's channel.
    /// </summary>
    [Column("dme_channel")]
    public string DMEChannel { get; set; }

    /// <summary>
    /// Gets or sets the DME's elevations (ft).
    /// </summary>
    [Column("dme_elevation_ft")]
    public int? DMEElevation { get; set; }

    /// <summary>
    /// Gets or sets the DME's latitiude.
    /// </summary>
    [Column("dme_latitude_deg")]
    public float? DMELat { get; set; }

    /// <summary>
    /// Gets or sets the DME's longitude.
    /// </summary>
    [Column("dme_longitude_deg")]
    public float? DMELon { get; set; }

    /// <summary>
    /// Gets or sets the elevation of the navaid (ft).
    /// </summary>
    [Column("elevation_ft")]
    public int? Elevation { get; set; }

    /// <summary>
    /// Gets or sets the frequency of the navaid (kHz).
    /// </summary>
    [Column("frequency_khz")]
    [Required]
    public int Freq { get; set; }

    /// <summary>
    /// Gets or sets the internal ID of the navaid.
    /// </summary>
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets the navaid's Ident.
    /// </summary>
    [Column("ident")]
    [Index]
    [MaxLength(8)] // Some muppet added NDB to the Ident (Wau_NDB)
    [Required]
    public string Ident { get; set; }

    /// <summary>
    /// Gets or sets the navaid's latitude.
    /// </summary>
    [Column("latitude_deg")]
    [Required]
    public float Lat { get; set; }

    /// <summary>
    /// Gets or sets the navaid's longitude.
    /// </summary>
    [Column("longitude_deg")]
    [Required]
    public float Lon { get; set; }

    /// <summary>
    /// Gets or sets the magenetic variation at the navaid's location (deg).
    /// </summary>
    [Column("magnetic_variation_deg")]
    public decimal? MagneticVariation { get; set; }

    /// <summary>
    /// Gets or sets the name of the navaid.
    /// </summary>
    [Column("name")]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the navaid's power.
    /// </summary>
    [Column("power")]
    public NavAidPowers? Power { get; set; }

    /// <summary>
    /// Gets or sets the navaid's salved magentic variation.
    /// </summary>
    [Column("slaved_variation_deg")]
    public decimal? SlavedVariation { get; set; }

    /// <summary>
    /// Gets or sets the type of navaid.
    /// </summary>
    [Column("type")]
    [Required]
    public NavAidTypes Type { get; set; }

    /// <summary>
    /// Gets or sets the navaid's usage type.
    /// </summary>
    [Column("usageType")]
    public NavAidUsages? UsageType { get; set; }
    #endregion
  }
}
