using eu.bayly.EADBasicNet.OpenFlights;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// The web service used for querying the OpenFlight databases.
  /// </summary>
  public class OpenFlightsController : ApiControllerBase {
    #region Properties
    /// <summary>
    /// Gets the namespace.
    /// </summary>
    protected override string Namespace {
      get {
        return "eu.bayly.EADBasicNet.OpenFlights";
      }
    }
    #endregion

    #region Methods
    #endregion

    #region Web methods
    /// <summary>
    /// Gets the list of airports.
    /// </summary>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = 86400)]
    [ResponseType(typeof(Airport[]))]
    public IHttpActionResult GetAirports(string country = null, string icao = null) {
      if (!string.IsNullOrEmpty(country))
        country = country.ToUpper();
      if (!string.IsNullOrEmpty(icao))
        icao = icao.ToUpper();

      try {
        var appData = GetAppData();
        var file = new FileInfo(Path.Combine(appData.FullName, "airports.txt"));

        IEnumerable<Airport> list = Airport.FromOpenFlights(file,
          s => ((country == null) || (s.Country == country)) && ((icao == null) || (s.ICAO == icao))
          );

        return Ok(list.ToArray());

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Gets the list of countries.
    /// </summary>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = 86400)]
    [ResponseType(typeof(string[]))]
    public IHttpActionResult GetCountries() {
      try {
        var appData = GetAppData();
        var file = new FileInfo(Path.Combine(appData.FullName, "airports.txt"));

        var list = (
        from a in Airport.FromOpenFlights(file)
        orderby a.Country
        select a.Country
        ).Distinct();

        return Ok(list);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }
    #endregion
  }
}