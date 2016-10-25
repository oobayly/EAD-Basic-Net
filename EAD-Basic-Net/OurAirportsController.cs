using eu.bayly.EADBasicNet.OurAirports;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// The web service used for querying the OurAirports database.
  /// </summary>
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
    /// Gets the icon for the specified airport.
    /// </summary>
    /// <param name="ident">The airport's ICAO ident.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    public HttpResponseMessage GetIcon(string ident) {
      if (string.IsNullOrEmpty(ident)) {
        return new HttpResponseMessage(HttpStatusCode.BadRequest); //BadRequest("ident is required.");
      }

      Airport airport;
      var db = new DataContext();
      airport = db.Airports
        .Where(x => x.Ident == ident)
        .Include("Frequencies")
        .Include("Runways")
        .OrderBy(a => a.Name)
        .FirstOrDefault();

      var svg = airport.GenerateIcon();
      var ms = new MemoryStream();
      svg.Save(ms);
      ms.Position = 0;

      var resp = new HttpResponseMessage(HttpStatusCode.OK);
      resp.Content = new StreamContent(ms);
      resp.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/svg+xml");

      return resp;
    }

    /// <summary>
    /// Gets the specified navaid.
    /// </summary>
    /// <param name="countryCode">The 2-character ISO code of the country in which the NavAid is located.</param>
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
    public async Task<IHttpActionResult> Import(string type) {
      var appData = GetAppData();
      await ImportAsync(new CancellationToken(), appData, type);
      return Ok();
    }

    /// <summary>
    /// Imports the data.
    /// </summary>
    [HttpGet]
    public IHttpActionResult ImportAsync(string type) {
      var appData = GetAppData();
      HostingEnvironment.QueueBackgroundWorkItem(ct => ImportAsync(ct, appData, type));
      return Ok();
    }

    private async Task<CancellationToken> ImportAsync(CancellationToken ct, DirectoryInfo appData, string type) {
      var db = new DataContext();

      if (string.IsNullOrEmpty(type)) {
        await db.ImportAllAsync(appData);
      } else {
        await db.ImportAsync(type, appData);
      }

      return ct;
    }

    /// <summary>
    /// Gets all the airports
    /// </summary>
    /// <param name="args">The parameters used for searching the airports database.</param>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = CacheDuration)]
    [ResponseType(typeof(Airport[]))]
    public IHttpActionResult SearchAirports([FromUri]AirportSearchArgs args) {
      try {
        var db = new DataContext();
        var airports = db.Airports
          .Where(x =>
            ((args.CountryCode == "") || (x.CountryCode == args.CountryCode))
            && ((args.RegionCode == "") || (x.RegionCode == args.RegionCode))
            && ((args.Ident == "") || (x.Ident == args.Ident))
            && ((args.Name == "") || x.Name.Contains(args.Name))
            && ((args.IdentOrName == "") || x.Ident.Contains(args.IdentOrName) || x.Name.Contains(args.IdentOrName))
            && (x.Lat >= args.South) && (x.Lat <= args.North) && (x.Lon >= args.West) && (x.Lon <= args.East)
            && ((args.HasScheduledService == null) || (x.ScheduledService == args.HasScheduledService))
            && ((x.Type & args.Type) != Airport.AirportTypes.None)
            )
          .Include("Frequencies")
          .OrderBy(x => x.Name)
          .ToList();

        return Ok(airports.Select(x => new {
          Ident = x.Ident,
          Lat = x.Lat,
          Lon = x.Lon,
          Name = x.Name,
          Type = x.Type,
          Frequencies = x.Frequencies,
          HasTower = x.HasTower
        }));

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
    public IHttpActionResult SearchNavAids([FromUri]NavAidSearchArgs args) {
      try {
        var db = new DataContext();
        var navaids = db.NavAids
          .Where(x =>
            ((args.CountryCode == "") || (x.CountryCode == args.CountryCode))
            && ((args.Ident == "") || (x.Ident == args.Ident))
            && ((args.Name == "") || (x.Name.Contains(args.Name)))
            && (x.Lat >= args.South) && (x.Lat <= args.North) && (x.Lon >= args.West) && (x.Lon <= args.East)
            && ((x.Power & args.Power) != NavAid.NavAidPowers.None)
            && ((x.Type & args.Type) != NavAid.NavAidTypes.None)
            && ((x.UsageType & args.UsageType) != NavAid.NavAidUsages.None)
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
