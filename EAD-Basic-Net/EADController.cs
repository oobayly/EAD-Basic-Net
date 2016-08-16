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
  public class EADController : ApiController {
    /// <summary>
    /// Gets a list of enum descriptors.
    /// </summary>
    [HttpPost]
    public Response<EnumDescriptor[]> GetEnums([FromBody]string name) {
      var response = new Response<EnumDescriptor[]>();

      try {
        response.Value = EnumDescriptor.GetDescriptors(name);
      } catch (Exception ex) {
        response.Error = ex.Message;
      }

      return response;
    }

    /// <summary>
    /// Searches the EAD website using the specified parameters.
    /// </summary>
    [HttpPost]
    [ResponseType(typeof(Document[]))]
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