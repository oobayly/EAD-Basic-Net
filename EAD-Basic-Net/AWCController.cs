using eu.bayly.EADBasicNet.AWC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// The web service used for querying the NOAA AWC website.
  /// </summary>
  public class AWCController : ApiControllerBase {
    #region Properties
    /// <summary>
    /// Gets the Enumeration namespace.
    /// </summary>
    protected override string Namespace {
      get { return "eu.bayly.EADBasicNet.AWC"; }
    }
    #endregion

    #region Web methods
    /// <summary>
    /// Gets the METAR observations for the specified location(s).
    /// </summary>
    [ResponseType(typeof(METARResponse))]
    public IHttpActionResult GetMETAR(string icao) {
      try {
        return Ok(METARResponse.FromAWS(icao));

      } catch (Exception ex) {
        return InternalServerError(ex);
      }
    }

    /// <summary>
    /// Gets the list of station codes.
    /// </summary>
    [HttpGet]
    [ResponseType(typeof(Station[]))]
    public IHttpActionResult GetStations(string country = null, string icao = null) {
      var appData = GetAppData();
      var file = new FileInfo(Path.Combine(appData.FullName, "stations.txt"));

      IEnumerable<Station> list = Station.FromAWS(file);
      if (!string.IsNullOrEmpty(country)) {
        country = country.ToUpper();
        list = (from s in list where s.Country == country select s);
      }
      if (!string.IsNullOrEmpty(icao)) {
        icao = icao.ToUpper();
        list = (from s in list where s.ICAO == icao select s);
      }

      return Ok(list.ToArray());
    }

    /// <summary>
    /// Gets the TAF observations for the specified location(s).
    /// </summary>
    [ResponseType(typeof(TAFResponse))]
    public IHttpActionResult GetTAF(string icao) {
      try {
        return Ok(TAFResponse.FromAWS(icao));

      } catch (Exception ex) {
        return InternalServerError(ex);
      }
    }
    #endregion
  }
}