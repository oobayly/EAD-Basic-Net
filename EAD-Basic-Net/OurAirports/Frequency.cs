using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents an OurAirports airport frequency record.
  /// </summary>
  [Table("frequencies")]
  [Uri("http://ourairports.com/data/airport-frequencies.csv")]
  public class Frequency {
    /// <summary>
    /// Gets or sets the description of the service the frequency provides.
    /// </summary>
    [Column("description")]
    [Required(AllowEmptyStrings =true)]
    public string Description { get; set; }

    /// <summary>
    /// Gets or setst the frequency (MHz).
    /// </summary>
    [Column("frequency_mhz")]
    [Required]
    public decimal Freq { get; set; }

    /// <summary>
    /// Gets or sets the internal ID of the frequency.
    /// </summary>
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets the Ident of the airport to which the frequency belongs.
    /// </summary>
    [Column("airport_ident")]
    [Index]
    [Required]
    public string Ident { get; set; }

    /// <summary>
    /// Gets or sets the type service the frequency provides.
    /// </summary>
    [Column("type")]
    [Required]
    public string Type { get; set; }
  }
}
