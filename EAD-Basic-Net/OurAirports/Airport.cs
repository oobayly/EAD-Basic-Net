using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Xml;

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
    [Flags]
    public enum AirportTypes {
      /// <summary>None</summary>
      None = 0,
      /// <summary>Any airport (used for searching)</summary>
      Any = 0xff,
      /// <summary>Balloon Port</summary>
      [Description("Balloon Port")]
      BalloonPort = 0x1,
      /// <summary>Closed</summary>
      Closed = 0x2,
      /// <summary>Heliport</summary>
      Heliport = 0x4,
      /// <summary></summary>
      [Description("Large Airport")]
      LargeAirport = 0x8,
      /// <summary>Medium Airport</summary>
      [Description("Medium Airport")]
      MediumAirport = 0x10,
      /// <summary>Seaplane Base</summary>
      [Description("Seaplane Base")]
      SeaplaneBase = 0x20,
      /// <summary>Small Airport</summary>
      [Description("Small Airport")]
      SmallAirport = 0x40
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
    /// Gets whether the airport has a tower.
    /// </summary>
    public bool HasTower {
      get {
        return (Frequencies != null) && Frequencies.Any(f => f.Type == "TWR");
      }
    }

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

    #region Methods
    private static PointF GetXY(double lat,  double lon) {
      return new PointF(
        (float)(65536 * ((Math.PI * lon / 180) + Math.PI) / Math.PI),
        (float)(65536 * (Math.PI - Math.Log(Math.Tan((Math.PI / 4) + (lat * Math.PI / 360)))) / Math.PI)
        );
    }

    /// <summary>
    /// Generate the icon for the airport.
    /// </summary>
    public XmlDocument GenerateIcon() {
      var doc = new System.Xml.XmlDocument();
      doc.LoadXml(System.Text.Encoding.UTF8.GetString(Properties.Resources.airport_icon));

      var root = doc.DocumentElement;

      var lyrRunways = (XmlElement)root.SelectSingleNode("//*[@id='lyrRunways']");
      var gRunways = (XmlElement)root.SelectSingleNode("//*[@id='gRunways']");
      var gRunwaysBG = (XmlElement)root.SelectSingleNode("//*[@id='gRunwaysBG']");

      var c = GetXY(Lat, Lon);
      double dist = 0;

      foreach (var rway in Runways) {
        if (rway.Closed || (rway.HighLat == null) || (rway.LowLat == null))
          continue;

        var h = GetXY(rway.HighLat.Value, rway.HighLon.Value);
        var l = GetXY(rway.LowLat.Value, rway.LowLon.Value);

        var dh = new PointF(h.X - c.X, h.Y - c.Y);
        var dl = new PointF(l.X - c.X, l.Y - c.Y);

        var disth = Math.Sqrt((dh.X * dh.X) + (dh.Y * dh.Y));
        var distl = Math.Sqrt((dl.X * dl.X) + (dl.Y * dl.Y));
        if (disth > dist) dist = disth;
        if (distl > dist) dist = distl;

        XmlElement elem;
        var path = string.Format("M {0},{1} {2},{3}", dh.X, dh.Y, dl.X, dl.Y);
        elem = doc.CreateElement("path", doc.DocumentElement.NamespaceURI);
        elem.SetAttribute("d", path);
        gRunwaysBG.AppendChild(elem);

        elem = doc.CreateElement("path", doc.DocumentElement.NamespaceURI);
        elem.SetAttribute("d", path);
        gRunways.AppendChild(elem);
      }

      double scale;
      if (Type == AirportTypes.LargeAirport) {
        scale = 15 / (2 * dist);
      } else {
        scale = 11 / (2 * dist);
      }
      gRunwaysBG.SetAttribute("style", string.Format("stroke-width: {0};", 2.5 / scale));
      gRunways.SetAttribute("style", string.Format("stroke-width: {0};", 1 / scale));

      lyrRunways.SetAttribute("transform", string.Format("matrix({0},0,0,{0},8,8.5)", scale));

      doc.DocumentElement.SetAttribute("class", Type.ToString().ToLower() + " " + (HasTower ? "has-tower" : "no-tower"));

      return doc;
    }
    #endregion
  }
}
