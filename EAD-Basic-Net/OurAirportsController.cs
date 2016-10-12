using eu.bayly.EADBasicNet.OurAirports;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace eu.bayly.EADBasicNet {
  public class OurAirportsController : ApiControllerBase {
    #region Constants
    /// <summary>
    /// The number of seconds that a response can be cached.
    /// </summary>
#if DEBUG
    protected const int CacheDuration = -1;
#else
    protected const int CacheDuration = 86400;
#endif
    #endregion

    #region Properties
    /// <summary>
    /// Gets the namespace.
    /// </summary>
    protected override string Namespace {
      get {
        return "eu.bayly.EADBasicNet.OurAirports";
      }
    }
    #endregion
    
    #region Web methods
    /// <summary>
    /// Gets the specified airport.
    /// </summary>
    /// <param name="ident">The airport's ICAO ident.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(Airport))]
    public IHttpActionResult GetAirport(string ident) {
      if (string.IsNullOrEmpty(ident)) {
        return BadRequest("ident is required.");
      }

      ident = ident.ToUpper();

      try {
        var db = new DataContext();
        var airport = db.Airports
          .Where(x => x.Ident == ident)
          .Include("Region.Country")
          .Include("Frequencies")
          .Include("NavAids")
          .Include("Runways")
          .OrderBy(a => a.Name)
          .FirstOrDefault();

        // Don't return a country value for every navaid
        foreach (var navaid in airport.NavAids) {
          navaid.Country = null;
        }

        return Ok(airport);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Gets the list of countries.
    /// </summary>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(Country[]))]
    public IHttpActionResult GetCountries() {
      try {
        var db = new DataContext();
        IQueryable<Country> countries = db.Countries
          .OrderBy(x => x.Name);

        return Ok(countries);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Gets the specified country.
    /// </summary>
    /// <param name="countryCode">The country's 2 character ISO code.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(Country))]
    public IHttpActionResult GetCountry(string countryCode) {
      if (string.IsNullOrEmpty(countryCode)) {
        return BadRequest("countryCode is required.");
      }

      countryCode = countryCode.ToUpper();

      try {
        var db = new DataContext();
        var countries = db.Countries
          .Where(x => x.Code == countryCode)
          .Include("Regions")
          .FirstOrDefault();

        return Ok(countries);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Gets the specified navaid.
    /// </summary>
    /// <param name="ident">The navaid's ICAO ident.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(NavAid))]
    public IHttpActionResult GetNavAid(string countryCode, string ident) {
      if (string.IsNullOrEmpty(countryCode)) {
        return BadRequest("countryCode is required.");
      } else if (string.IsNullOrEmpty(ident)) {
        return BadRequest("ident is required.");
      }

      countryCode = countryCode.ToUpper();
      ident = ident.ToUpper();

      try {
        var db = new DataContext();
        var navaid = db.NavAids
          .Where(x => (x.CountryCode == countryCode) && (x.Ident == ident))
          .Include("Country")
          .OrderBy(a => a.Name)
          .FirstOrDefault();

        return Ok(navaid);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Gets the specified region.
    /// </summary>
    /// <param name="regionCode">The region's ISO code.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(Region))]
    public IHttpActionResult GetRegion(string regionCode) {
      if (string.IsNullOrEmpty(regionCode)) {
        return BadRequest("regionCode is required.");
      }

      regionCode = regionCode.ToUpper();

      try {
        var db = new DataContext();
        var regions = db.Regions
          .Where(x => x.Code == regionCode)
          .Include("Airports")
          .FirstOrDefault();

        return Ok(regions);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Gets the regions for the specified country.
    /// </summary>
    /// <param name="countryCode">The country's 2 character ISO code.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(Region[]))]
    public IHttpActionResult GetRegions(string countryCode) {
      if (string.IsNullOrEmpty(countryCode)) {
        return BadRequest("countryCode is required.");
      }

      countryCode = countryCode.ToUpper();

      try {
        var db = new DataContext();
        var regions = db.Regions
          .Where(x => x.CountryCode == countryCode)
          .OrderBy(x => x.Name);

        return Ok(regions);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Imports the data.
    /// </summary>
    [HttpGet]
    public IHttpActionResult Import(string type) {
      try {
        var appData = GetAppData();

        var db = new DataContext();

        if (string.IsNullOrEmpty(type)) {
          db.ImportAll(appData);
        } else {
          db.Import(type, appData);
        }

        return Ok();
        //return Ok(db.Countries.ToArray());

      } catch (Exception ex) {
        throw;
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Gets all the airports
    /// </summary>
    /// <param name="args">The parameters used for searching the airports database.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(Airport[]))]
    public IHttpActionResult SearchAirports([FromUri]SearchArgs args) {
      try {
        var db = new DataContext();
        var airports = db.Airports
          .Where(x =>
            ((args.CountryCode == "") || (x.CountryCode == args.CountryCode))
            && ((args.RegionCode == "") || (x.RegionCode == args.RegionCode))
            && ((args.Ident == "") || (x.Ident == args.Ident))
            && ((args.Name == "") || (x.Name.Contains(args.Name)))
            && (x.Lat >= args.LatMin) && (x.Lat <= args.LatMax) && (x.Lon >= args.LonMin) && (x.Lon <= args.LonMax)
            )
          .OrderBy(x => x.Name)
          .Select(x => new {
            Ident = x.Ident,
            Lat = x.Lat,
            Lon = x.Lon,
            Name = x.Name
          });

        return Ok(airports);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Search NavAids.
    /// </summary>
    /// <param name="args">The parameters used for searching the navaids database.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(NavAid[]))]
    public IHttpActionResult SearchNavAids([FromUri]SearchArgs args) {
      try {
        var db = new DataContext();
        var navaids = db.NavAids
          .Where(x =>
            ((args.CountryCode == "") || (x.CountryCode == args.CountryCode))
            && ((args.Ident == "") || (x.Ident == args.Ident))
            && ((args.Name == "") || (x.Name.Contains(args.Name)))
            && (x.Lat >= args.LatMin) && (x.Lat <= args.LatMax) && (x.Lon >= args.LonMin) && (x.Lon <= args.LonMax)
            )
          .OrderBy(a => a.Name)
          .Select(a => new {
            Ident = a.Ident,
            Lat = a.Lat,
            Lon = a.Lon,
            Name = a.Name
          });

        return Ok(navaids);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }
    #endregion
  }
}
