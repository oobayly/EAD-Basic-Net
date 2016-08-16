using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using eu.bayly.EADBasicNet.EAD;

namespace eu.bayly.EADBasicNet {
  public class EADController : ApiController {
    /// <summary>
    /// Searches the EAD website using the specified parameters.
    /// </summary>
    public Response<Document[]> Search([FromBody]SearchArgs args) {
      var response = new Response<Document[]>();

      try {
        response.Value = Document.Search(args).ToArray();
      } catch (Exception ex) {
        response.Error = ex.Message;
      }

      return response;
    }
  }
}