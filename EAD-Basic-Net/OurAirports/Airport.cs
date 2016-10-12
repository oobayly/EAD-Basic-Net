using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents an OurAirports airport record.
  /// </summary>
  [Table("airports")]
  [Uri("http://ourairports.com/data/airports.csv")]
  public class Airport {
    #region Enumerations
    /// <summary>
    /// Represents a type of Airport.
    /// </summary>
    public enum AirportTypes {
      /// <summary>Balloon Port</summary>
      [Description("Balloon Port")]
      BalloonPort,
      /// <summary>Closed</summary>
      Closed,
      /// <summary>Heliport</summary>
      Heliport,
      /// <summary></summary>
      [Description("Large Airport")]
      LargeAirport,
      /// <summary>Medium Airport</summary>
      [Description("Medium Airport")]
      MediumAirport,
      /// <summary>Seaplane Base</summary>
      [Description("Seaplane Base")]
      SeaplaneBase,
      /// <summary>Small Airport</summary>
      [Description("Small Airport")]
      SmallAirport
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the 2-character ISO contintent code.
    /// </summary>
    [Column("continent")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string Continent { get; set; }

    /// <summary>
    /// Gets or sets the 2-character ISO country code.
    /// </summary>
    [Column("iso_country")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string CountryCode { get; set; }

    /// <summary>
    /// Gets or sets the airport's elevation (feet).
    /// </summary>
    [Column("elevation_ft")]
    public int? Elevation { get; set; }

    /// <summary>
    /// Gets or sets the list of the airport's radio frequencies.
    /// </summary>
    public ICollection<Frequency> Frequencies { get; set; }

    /// <summary>
    /// Gets or sets the airport's 3-character IATA code'
    /// </summary>
    [Column("iata_code")]
    [MaxLength(3)]
    [Index]
    public string IATA { get; set; }

    /// <summary>
    /// Gets or sets the airport's unique Ident.
    /// </summary>
    [Column("ident")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public string Ident { get; set; }

    /// <summary>
    /// Gets or sets the airport's GPS code.
    /// </summary>
    [Column("gps_code")]
    [MaxLength(4)]
    public string GPSCode { get; set; }

    /// <summary>
    /// Gets or sets the URL to the airport's home page.
    /// </summary>
    [Column("home_link")]
    public string HomeLink { get; set; }

    /// <summary>
    /// Gets or sets any keywords used to describe the airport.
    /// </summary>
    [Column("keywords")]
    public string Keywords { get; set; }

    /// <summary>
    /// Gets or sets the airports latitude.
    /// </summary>
    [Column("latitude_deg")]
    [Required]
    public float Lat { get; set; }

    /// <summary>
    /// Gets or sets the airport's local code.
    /// </summary>
    [Column("local_code")]
    public string LocalCode { get; set; }

    /// <summary>
    /// Gets or sets the airports longitude.
    /// </summary>
    [Column("longitude_deg")]
    [Required]
    public float Lon { get; set; }

    /// <summary>
    /// Gets or sets the name of the municipality in which the airport is located..
    /// </summary>
    [Column("municipality")]
    public string Municipality { get; set; }

    /// <summary>
    /// Gets or sets the airports name.
    /// </summary>
    [Column("name")]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the list of the navaids attached to the airport.
    /// </summary>
    public ICollection<NavAid> NavAids { get; set; }

    /// <summary>
    /// Gets or sets the region in which the airport is located.
    /// </summary>
    [ForeignKey("RegionCode")]
    public Region Region { get; set; }

    /// <summary>
    /// Gets or sets the 5-character ISO region code.
    /// </summary>
    [Column("iso_region")]
    [Index]
    [Required]
    public string RegionCode { get; set; }

    /// <summary>
    /// Gets or sets the list of the runways int the airport.
    /// </summary>
    public ICollection<Runway> Runways { get; set; }

    /// <summary>
    /// Gets or sets whether the airport has scheduled services.
    /// </summary>
    [Column("scheduled_service")]
    public bool ScheduledService { get; set; }

    /// <summary>
    /// Gets or sets the airport type.
    /// </summary>
    [Column("type")]
    [Required]
    public AirportTypes Type { get; set; }

    /// <summary>
    /// Gets or sets the URL to the airport's wikipedia article.
    /// </summary>
    [Column("wikipedia_link")]
    public string WikipediaLink { get; set; }
    #endregion
  }
}
