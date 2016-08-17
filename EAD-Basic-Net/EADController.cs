using eu.bayly.EADBasicNet.EAD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// The web service used for querying the EAD database.
  /// </summary>
  public class EADController : ApiControllerBase {
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

        var list = new Dictionary<Enum, string>();
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
        return Ok(args.Search().ToArray());

      } catch (WebException ex) {
        var resp = ex.Response as HttpWebResponse;
        string message;
        if (resp == null) {
          message = "The remote server returned an error.";
        } else {
          message = string.Format("The remote server returned a {0} error.", resp.StatusCode);
        }
        return InternalServerError(ex, message);

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }
  }
}