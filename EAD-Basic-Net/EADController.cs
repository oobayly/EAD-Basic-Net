using eu.bayly.EADBasicNet.EAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [HttpPost]
    [ResponseType(typeof(EnumDescriptor[]))]
    public IHttpActionResult GetEnums([FromBody]string name) {

      try {
        return Ok(EnumDescriptor.GetDescriptors(name));

      } catch (NullReferenceException) {
        return BadRequest(string.Format("'{0}' is not a valid enumeration.", name));

      } catch (Exception ex) {
        return InternalServerError(ex);

      }
    }

    /// <summary>
    /// Searches the EAD website using the specified parameters.
    /// </summary>
    [HttpPost]
    [ResponseType(typeof(Document[]))]
    public IHttpActionResult Search([FromBody]SearchArgs args) {
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