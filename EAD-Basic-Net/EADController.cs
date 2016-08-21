using eu.bayly.EADBasicNet.EAD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// The web service used for querying the EAD website.
  /// </summary>
  public class EADController : ApiControllerBase {
    #region Methods
    private Document[] Search(SearchArgs args, out bool fromCache) {
      Document[] list = null;
      fromCache = false;

      // Compute the hash and check if it exists
      var hash = args.MD5Sum();

      var appData = GetAppData();
      foreach (var f in appData.GetFiles()) {
        if (f.LastWriteTime < DateTime.Now.AddDays(-1)) {
          f.Delete();
        }
      }

      var file = new FileInfo(Path.Combine(appData.FullName, hash + ".json"));
      if (file.Exists) {
        var found = CachedSearch<Document[]>.FromFile(file);
        if (found.IsValid) {
          // Mark the respnse as cached and return those documents.
          fromCache = true;
          list = found.Value;

        } else {
          // Remove the results if no longer valid.
          file.Delete();

        }
      }

      if (list == null) {
        list = args.Search();

        if (list.Length != 0) {
          var cached = new CachedSearch<Document[]>() {
            Value = list,
            MD5Sum = args.MD5Sum(),
            Timestamp = list.First().Timestamp
          };
          cached.Write(file);

        }
      }

      return list;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Gets the namespace.
    /// </summary>
    protected override string Namespace {
      get { return "eu.bayly.EADBasicNet.EAD"; }
    }
    #endregion

    #region Web methods
    /// <summary>
    /// Gets the list of ICAO codes for the specified authority.
    /// </summary>
    [HttpGet]
    [CacheOutput(ClientTimeSpan = 86400)]
    [ResponseType(typeof(string[]))]
    public IHttpActionResult GetICAO(Authority authority) {
      var args = new SearchArgs() {
        Authority = authority,
        PartAIRAC = PartAIRAC.AD
      };

      try {
        bool fromCache;
        var docs = Search(args, out fromCache);

        if (fromCache)
          HttpContext.Current.Response.AddHeader("X-Cached", true.ToString());

        var list = (from d in docs
                    where d.ICAO != null
                    select d.ICAO).Distinct();

        return Ok(list);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Searches the EAD website using the specified parameters.
    /// </summary>
    [HttpPost]
    [ResponseType(typeof(Document[]))]
    public IHttpActionResult Search([FromBody] SearchArgs args) {
      try {
        bool fromCache;
        var docs = Search(args, out fromCache);

        if (fromCache)
          HttpContext.Current.Response.AddHeader("X-Cached", true.ToString());

        return Ok(docs);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }
    #endregion
  }
}