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
    /// Gets or sets most Easterly point to search for.
    /// </summary>
    public float East { get; set; }

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
    /// Gets or sets the name to be searched for.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets most Northerly point to search for.
    /// </summary>
    public float North { get; set; }

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
    /// Gets or sets most Southerly point to search for.
    /// </summary>
    public float South { get; set; }

    /// <summary>
    /// Gets or sets most Westerly point to search for.
    /// </summary>
    public float West { get; set; }

    /// <summary>
    /// Creates an instance of the SearchArgs class.
    /// </summary>
    public SearchArgs() {
      CountryCode = "";
      Ident = "";
      North = 90;
      South = -90;
      East = 180;
      West = -180;
      Name = "";
      RegionCode = "";
    }
  }
}