using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents OurAirport search arguments for an airport or navaid.
  /// </summary>
  public class SearchArgs {
    private string countryCode, ident, regionCode;

    /// <summary>
    /// Gets or sets the country code to be searched for.
    /// </summary>
    public string CountryCode {
      get {
        return countryCode;
      }
      set {
        if (value != null)
          value = value.ToUpper();
        countryCode = value;
      }
    }

    /// <summary>
    /// Gets or sets the ident to be searched for.
    /// </summary>
    public string Ident {
      get {
        return ident;
      }
      set {
        if (value != null)
          value = value.ToUpper();
        ident = value;
      }
    }


    /// <summary>
    /// Gets or sets maximum latitude.
    /// </summary>
    public float LatMax { get; set; }

    /// <summary>
    /// Gets or sets minimum latitude.
    /// </summary>
    public float LatMin { get; set; }

    /// <summary>
    /// Gets or sets maximum longitude.
    /// </summary>
    public float LonMax { get; set; }

    /// <summary>
    /// Gets or sets minimum latitude.
    /// </summary>
    public float LonMin { get; set; }

    /// <summary>
    /// Gets or sets the name to be searched for.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the region code to be searched for.
    /// </summary>
    public string RegionCode {
      get {
        return regionCode;
      }
      set {
        if (value != null)
          value = value.ToUpper();
        regionCode = value;
      }
    }


    /// <summary>
    /// Creates an instance of the SearchArgs class.
    /// </summary>
    public SearchArgs() {
      CountryCode = "";
      Ident = "";
      LatMax = 90;
      LatMin = -90;
      LonMax = 180;
      LonMin = -180;
      Name = "";
      RegionCode = "";
    }
  }
}