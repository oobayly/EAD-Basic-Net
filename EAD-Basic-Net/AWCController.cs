﻿using eu.bayly.EADBasicNet.AWC;
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
  /// The web service used for querying the NOAA AWC website.
  /// </summary>
  public class AWCController : ApiControllerBase {
    #region Properties
    /// <summary>
    /// Gets the namespace.
    /// </summary>
    protected override string Namespace {
      get { return "eu.bayly.EADBasicNet.AWC"; }
    }
    #endregion

    #region Web methods
    /// <summary>
    /// Gets the METAR observations for the specified location(s).
    /// </summary>
    [HttpPost]
    [ResponseType(typeof(METARResponse))]
    public IHttpActionResult GetMETAR([FromBody]SearchArgs args) {
      try {
        return Ok(args.Search<METAR>());

      } catch (Exception ex) {
        return InternalServerError(ex);
      }
    }

    /// <summary>
    /// Gets the list of station codes.
    /// </summary>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = 86400)]
    [ResponseType(typeof(Station[]))]
    public IHttpActionResult GetStations(string country = null, string icao = null) {
      var appData = GetAppData();
      var file = new FileInfo(Path.Combine(appData.FullName, "stations.txt"));

      if (!string.IsNullOrEmpty(country))
        country = country.ToUpper();
      if (!string.IsNullOrEmpty(icao))
        icao = icao.ToUpper();

      IEnumerable<Station> list = Station.FromAWS(file,
        s => ((country == null) || (s.Country == country)) && ((icao == null) || (s.ICAO == icao))
        );

      return Ok(list.ToArray());
    }

    /// <summary>
    /// Gets the TAF observations for the specified location(s).
    /// </summary>
    [HttpPost]
    [ResponseType(typeof(TAFResponse))]
    public IHttpActionResult GetTAF([FromBody]SearchArgs args) {
      try {
        return Ok(args.Search<TAF>());

      } catch (Exception ex) {
        return InternalServerError(ex);
      }
    }
    #endregion
  }
}