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

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// The web service used for querying the EAD website.
  /// </summary>
  public class EADController : ApiControllerBase {
    #region Methods
    #endregion

    #region Properties
    /// <summary>
    /// Gets the Enumeration namespace.
    /// </summary>
    protected override string Namespace {
      get { return "eu.bayly.EADBasicNet.EAD"; }
    }
    #endregion

    #region Web methods
    /// <summary>
    /// Searches the EAD website using the specified parameters.
    /// </summary>
    [HttpPost]
    [ResponseType(typeof(Document[]))]
    public IHttpActionResult Search([FromBody] SearchArgs args) {
      try {
        Document[] list = null;

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
          var found = CachedSearch.FromFile(file);
          if (found.IsValid) {
            // Mark the respnse as cached and return those documents.
            HttpContext.Current.Response.AddHeader("X-Cached", true.ToString());
            list = found.Documents;

          } else {
            // Remove the results if no longer valid.
            file.Delete();

          }
        }

        if (list == null) {
          list = args.Search();

          if (list.Length != 0) {
            var cached = new CachedSearch() {
              Documents = list,
              MD5Sum = args.MD5Sum(),
              Timestamp = list.First().Timestamp
            };
            cached.Write(file);

          }
        }

        return Ok(list);

      } catch (Exception ex) {
        return Ok(new {
          Message = ex.Message,
          Type = ex.GetType(),
          StackTrace = ex.StackTrace
        });
//        return InternalServerError(ex);

      }
    }
    #endregion
  }
}