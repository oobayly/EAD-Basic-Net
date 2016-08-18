using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// Extends properties and methods on the stock ApiController.
  /// </summary>
  public abstract class ApiControllerBase : ApiController {
    /// <summary>
    /// Gets the Enumeration namespace.
    /// </summary>
    protected abstract string Namespace { get; }

    /// <summary>
    /// Gets the AppData directory for the current controller.
    /// </summary>
    /// <returns></returns>
    protected DirectoryInfo GetAppData() {
      var dir = new DirectoryInfo(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/App_Data"), Namespace));
      if (!dir.Exists) {
        dir.Create();
        dir.Refresh();
      }

      return dir;
    }

    /// <summary>
    /// Gets the enums for the specified type.
    /// </summary>
    [HttpGet]
    [ResponseType(typeof(Dictionary<Enum, string>))]
    public IHttpActionResult GetEnums(string name) {
      var type = Type.GetType(Namespace + "." + name);
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
    /// Creates an System.Web.Http.Results.InternalServerErrorResult (500 Internal Server Error).
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <returns>A System.Web.Http.Results.InternalServerErrorResult.</returns>
    protected System.Web.Http.Results.ExceptionResult InternalServerError(string message) {
      return base.InternalServerError(new Exception(message));
    }

    /// <summary>
    /// Creates an System.Web.Http.Results.InternalServerErrorResult (500 Internal Server Error).
    /// </summary>
    /// <param name="exception">The exception to include in the error.</param>
    /// <returns>A System.Web.Http.Results.InternalServerErrorResult.</returns>
    protected override System.Web.Http.Results.ExceptionResult InternalServerError(Exception exception) {
#if DEBUG
      return base.InternalServerError(exception);
#else
      string message;

      if (exception is EAD.EADException) {
        message = string.Format("The EAD website returned the following error: {0}", exception.Message);

      } else if (exception is IOException) {
        message = string.Format("An error occurred accessing the local file system: {0}", exception.Message);

      } else if (exception is WebException) {
        var resp = ((WebException)exception).Response as HttpWebResponse;
        if (resp == null) {
          message = "The remote server returned an error.";
        } else {
          message = string.Format("The remote server returned a {0} error.", resp.StatusCode);
        }

      } else {
        message = "An unexpected error occurred.";

      }

      return InternalServerError(message);
#endif
    }

    /// <summary>
    /// Creates an System.Web.Http.Results.InternalServerErrorResult (500 Internal Server Error).
    /// </summary>
    /// <param name="ex">The exception to include in the error.</param>
    /// <param name="message">The exception message.</param>
    /// <returns>A System.Web.Http.Results.InternalServerErrorResult.</returns>
    protected System.Web.Http.Results.ExceptionResult InternalServerError(Exception ex, string message) {
#if DEBUG
      return base.InternalServerError(ex);
#else
      return InternalServerError(message);
#endif
    }
  }
}