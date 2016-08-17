using System;
using System.Net;
using System.Web.Http;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// Extends properties and methods on the stock ApiController.
  /// </summary>
  public abstract class ApiControllerBase : ApiController {
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