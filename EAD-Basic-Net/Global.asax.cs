using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace eu.bayly.EADBasicNet {
  public class Global : System.Web.HttpApplication {

    protected void Application_Start(object sender, EventArgs e) {
      // Used for registering the Web API routes
      GlobalConfiguration.Configure(WebApiConfig.Register);

      // Used for Web API Help generation
      AreaRegistration.RegisterAllAreas();
    }

    protected void Session_Start(object sender, EventArgs e) {
    }

    protected void Application_BeginRequest(object sender, EventArgs e) {
      // Some web hosts disable web.config options - namely system.webServer/httpProtocol/
      // So create the CORS headers here instead
      if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/api")) {
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

        if (HttpContext.Current.Request.HttpMethod == "OPTIONS") {
          //These headers are handling the "pre-flight" OPTIONS call sent by the browser
          HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
          HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
          HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
          HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "Content-Disposition, Last-Modified");
          HttpContext.Current.Response.End();
        }
      }
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e) {
    }

    protected void Application_Error(object sender, EventArgs e) {
    }

    protected void Session_End(object sender, EventArgs e) {
    }

    protected void Application_End(object sender, EventArgs e) {
    }
  }
}
