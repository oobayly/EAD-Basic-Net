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
    /// Gets the list of station codes.
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    [HttpGet]
    [ResponseType(typeof(Station[]))]
    public IHttpActionResult GetStations(string country = null, string icao = null) {
      var path = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/App_Data"));
      var file = new FileInfo(Path.Combine(path.FullName, "stations.txt"));

      IEnumerable<Station> list = Station.FromFile(file);
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
    #endregion
  }
}