using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  [Table("airports")]
  [Uri("http://ourairports.com/data/airports.csv")]
  public class Airport {
    #region Enumerations
    public enum AirportTypes {
      BalloonPort,
      Closed,
      Heliport,
      LargeAirport,
      MediumAirport,
      SeaplaneBase,
      SmallAirport
    }
    #endregion

    #region Properties
    [Column("continent")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string Continent { get; set; }

    [ForeignKey("CountryCode")]
    public Country Country { get; set; }

    [Column("iso_country")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string CountryCode { get; set; }

    [Column("elevation_ft")]
    public int? Elevation { get; set; }

    public ICollection<Frequency> Frequencies { get; set; }

    [Column("iata_code")]
    [MaxLength(3)]
    [Index]
    public string IATA { get; set; }

    [Column("ident")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public string Ident { get; set; }

    [Column("gps_code")]
    [MaxLength(4)]
    public string GPSCode { get; set; }

    [Column("home_link")]
    public string HomeLink { get; set; }

    [Column("keywords")]
    public string Keywords { get; set; }

    [Column("latitude_deg")]
    [Required]
    public float Lat { get; set; }

    [Column("local_code")]
    public string LocalCode { get; set; }

    [Column("longitude_deg")]
    [Required]
    public float Lon { get; set; }

    [Column("municipality")]
    public string Municipality { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    public ICollection<NavAid> NavAids { get; set; }

    [ForeignKey("RegionCode")]
    public Region Region { get; set; }

    [Column("iso_region")]
    [Index]
    [Required]
    public string RegionCode { get; set; }

    public ICollection<Runway> Runways { get; set; }

    [Column("scheduled_service")]
    public bool ScheduledService { get; set; }

    [Column("type")]
    [Required]
    public AirportTypes Type { get; set; }

    [Column("wikipedia_link")]
    public string WikipediaLink { get; set; }
    #endregion
  }
}
