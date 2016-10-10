using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  [Table("countries")]
  [Uri("http://ourairports.com/data/countries.csv")]
  public class Country {
    public ICollection<Airport> Airports { get; set; }

    [Column("code")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [MaxLength(2)]
    [Required]
    public string Code { get; set; }

    [Column("continent")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string Continent { get; set; }

    [Column("keywords")]
    public string Keywords { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; }

    public ICollection<Region> Regions { get; set; }

    [Column("wikipedia_link")]
    public string WikipediaLink { get; set; }
  }
}
