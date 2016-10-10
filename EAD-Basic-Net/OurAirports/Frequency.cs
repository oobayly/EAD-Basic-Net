using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eu.bayly.EADBasicNet.OurAirports {
  [Table("frequencies")]
  [Uri("http://ourairports.com/data/airport-frequencies.csv")]
  public class Frequency {
    [ForeignKey("Ident")]
    public Airport Airport { get; set; }

    [Column("description")]
    [Required(AllowEmptyStrings =true)]
    public string Description { get; set; }

    [Column("frequency_mhz")]
    [Required]
    public decimal Freq { get; set; }

    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Key]
    [Required]
    public int ID { get; set; }

    [Column("airport_ident")]
    [Index]
    [Required]
    public string Ident { get; set; }

    [Column("type")]
    [Required]
    public string Type { get; set; }
  }
}
