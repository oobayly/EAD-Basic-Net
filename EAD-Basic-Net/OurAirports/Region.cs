using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents an OurAirports region record.
  /// </summary>
  [Table("regions")]
  [Uri("http://ourairports.com/data/regions.csv")]
  public class Region {
    /// <summary>
    /// Gets or sets the list of airports in the region.
    /// </summary>
    public ICollection<Airport> Airports { get; set; }

    /// <summary>
    /// Gets or sets the 5-character ISO region code.
    /// </summary>
    [Column("code")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the 2-character ISO contintent code.
    /// </summary>
    [Column("continent")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string Continent { get; set; }

    /// <summary>
    /// Gets or sets the country in which the region is located.
    /// </summary>
    [ForeignKey("CountryCode")]
    public Country Country { get; set; }

    /// <summary>
    /// Gets or sets the 2-character ISO code of the country in which the region is located.
    /// </summary>
    [Column("iso_country")]
    [Index]
    [MaxLength(2)]
    [Required]
    public string CountryCode { get; set; }

    /// <summary>
    /// Gets or sets any keywords used to describe the region.
    /// </summary>
    [Column("keywords")]
    public string Keywords { get; set; }

    /// <summary>
    /// Gets or sets the local code of the region.
    /// </summary>
    [Column("local_code")]
    public string LocalCode { get; set; }

    /// <summary>
    /// Gets or sets the name of the region.
    /// </summary>
    [Column("name")]
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL to the region's wikipedia article.
    /// </summary>
    [Column("wikipedia_link")]
    public string WikipediaLink { get; set; }
  }
}
