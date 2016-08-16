using System;
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
