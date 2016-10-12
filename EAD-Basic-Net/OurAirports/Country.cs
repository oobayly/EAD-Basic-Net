using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents an OurAirports country record.
  /// </summary>
  [Table("countries")]
  [Uri("http://ourairports.com/data/countries.csv")]
  public class Country {
    /// <summary>
    /// Gets or sets the 2-character ISO country code.
    /// </summary>
    [Column("code")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [MaxLength(2)]
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the 2-character ISO continent code.
    /// </summary>
    [Column("continent")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string Continent { get; set; }

    /// <summary>
    /// Gets or sets any keywords used to describe the airport.
    /// </summary>
    [Column("keywords")]
    public string Keywords { get; set; }

    /// <summary>
    /// Gets or sets the airports name.
    /// </summary>
    [Column("name")]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a list of regions in the country.
    /// </summary>
    public ICollection<Region> Regions { get; set; }

    /// <summary>
    /// Gets or sets the URL to the country's wikipedia article.
    /// </summary>
    [Column("wikipedia_link")]
    public string WikipediaLink { get; set; }
  }
}
