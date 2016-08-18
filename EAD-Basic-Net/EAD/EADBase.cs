using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace eu.bayly.EADBasicNet.EAD {
  /// <summary>
  /// A base class which provides access to the EAD website.
  /// </summary>
  public abstract class EADBase {
    #region Constants
    /// <summary>
    /// The uri where login credentials should be posted.
    /// </summary>
    protected const string LoginUri = "http://ead-website.ead-it.com/pamslight/login.do";

    /// <summary>
    /// The uri which is contains the search options.
    /// </summary>
    protected const string OptionsUri = "http://ead-website.ead-it.com/pamslight/protect/options.do;jsessionid=";

    /// <summary>
    /// The uri which is used to search EAD Basic.
    /// </summary>
    protected const string SearchUri = "http://ead-website.ead-it.com/pamslight/protect/query.do;jsessionid=";
    #endregion

    #region Properties
    /// <summary>
    /// Gets whether the EAD website session has expired.
    /// </summary>
    protected static bool HasExpired {
      get {
        if (string.IsNullOrEmpty(Session) || !ID.HasValue)
          return true;

        if (LastAuth.HasValue) {
          return DateTime.Now.AddHours(-1) > LastAuth.Value;
        } else {
          return true;
        }
      }
    }

    /// <summary>
    /// Gets or sets the ID used for making requests.
    /// </summary>
    public static long? ID { get; private set; }

    /// <summary>
    /// Gets or sets the time when the last authentication was successful.
    /// </summary>
    protected static DateTime? LastAuth { get; private set; }

    /// <summary>
    /// Gets or sets the session string for making requests.
    /// </summary>
    protected static string Session { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Authenticates with the EAD website.
    /// </summary>
    protected bool Authenticate() {
      using (var client = new WebClient()) {
        string response;
        Match m;
        
        // Attempt to get the session
        response = client.DownloadString(LoginUri + "?user=guest&password=guest");
        if (!(m = Regex.Match(response, "redirecturl(\\s*)=(.*?)=(?<Session>.+?)\"")).Success) 
          return false;

        var session = m.Groups["Session"].Value;

        // Attempt to get the timestamp ID for all requests
        response = client.DownloadString(OptionsUri + session + "?org=");
        if (!(m = Regex.Match(response, "name=\"id\".*value=\"(?<ID>[0-9]+)\"")).Success)
          return false;

        LastAuth = DateTime.Now;
        Session = session;
        ID = long.Parse(m.Groups["ID"].Value);
      }

      return true;
    }

    /// <summary>
    /// Creates the HttpWebRequest for the specified Uri.
    /// </summary>
    protected abstract HttpWebRequest CreateRequest(string uri, object args);

    /// <summary>
    /// Makes a request to the EAD website.
    /// </summary>
    /// <param name="uri">The Uri being reqested.</param>
    /// <param name="args">The optional arguments to EADBase.CreateRequest.</param>
    /// <exception cref="EADException">The EAD website returned an error.</exception>
    protected string MakeRequest(string uri, object args = null) {
      // Authenticate only if session expired
      if (HasExpired && !Authenticate()) {
        throw new WebException("Not authenticated.");
      }

      var request = CreateRequest(uri, args);
      string text;
      using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
        using (var stream = response.GetResponseStream()) {
          using (var reader = new StreamReader(stream)) {
            text = reader.ReadToEnd();
          }
        }
      }

      // Test if the session has expired (PAMS-PU-003: Access denied)
      if (text.Contains("PAMS-PU-003")) {
        return MakeRequest(uri, args);
      } else {
        return text;
      }
    }
    #endregion
  }
}