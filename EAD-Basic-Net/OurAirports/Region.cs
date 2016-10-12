using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  [Table("regions")]
  [Uri("http://ourairports.com/data/regions.csv")]
  public class Region {
    public ICollection<Airport> Airports { get; set; }

    [Column("code")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public string Code { get; set; }

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

    [Column("keywords")]
    public string Keywords { get; set; }

    [Column("local_code")]
    public string LocalCode { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    [Column("wikipedia_link")]
    public string WikipediaLink { get; set; }
  }
}
