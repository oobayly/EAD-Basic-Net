using eu.bayly.EADBasicNet.EAD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// The web service used for querying the EAD database.
  /// </summary>
  public class EADController : ApiControllerBase {
    #region Methods
    #endregion

    #region Web methods
    /// <summary>
    /// Gets a list of enum descriptors.
    /// </summary>
    [HttpGet]
    [ResponseType(typeof(Dictionary<Enum, string>))]
    public IHttpActionResult GetEnums(string name) {
      var type = Type.GetType("eu.bayly.EADBasicNet.EAD." + name);
      if (type == null)
        return BadRequest(string.Format("'{0}' is not a valid enumeration.", name));

      try {
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        var list = new System.Collections.Hashtable();
        for (int i = 0; i < fields.Length; i++) {
          var desc = fields[i].GetCustomAttribute<DescriptionAttribute>();
          Enum value = (Enum)fields[i].GetValue(null);
          list[value] = desc == null ? fields[i].Name : desc.Description;
        }

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
        Document[] list = null;

        // Compute the hash and check if it exists
        var hash = args.MD5Sum();

        var path = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/App_Data"));
        foreach (var f in path.GetFiles()) {
          if (f.LastWriteTime < DateTime.Now.AddDays(-1)) {
            f.Delete();
          }
        }

        var file = new FileInfo(Path.Combine(path.FullName, hash + ".json"));
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