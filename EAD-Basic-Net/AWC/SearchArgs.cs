using System;
using System.Net;

namespace eu.bayly.EADBasicNet.AWC {
  /// <summary>
  /// Represents AWC search argments.
  /// </summary>
  [Areas.HelpPage.ModelDescriptions.ModelName("AWC.SearchArgs")]
  public class SearchArgs {
    #region Constants
    /// <summary>
    /// The Uri which contains the observation data.
    /// </summary>
    public const string ReportUri = "https://aviationweather.gov/adds/dataserver_current/httpparam?";
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the search end time.
    /// </summary>
    [QueryString("endTime")]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Gets or sets the number of hours before the current time to search for.
    /// </summary>
    [QueryString("hoursBeforeNow")]
    public float? HoursBeforeNow { get; set; }

    /// <summary>
    /// Gets or sets whether only the most recent result should be returned.
    /// </summary>
    public MostRecentType MostRecent { get; set; }

    /// <summary>
    /// Gets or sets the station to search for.
    /// </summary>
    [QueryString("stationString")]
    public string[] Stations { get; set; }

    /// <summary>
    /// Gets or sets the search start time.
    /// </summary>
    [QueryString("startTime")]
    public DateTime? StartTime { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates an instance of the SearchArgs class.
    /// </summary>
    public SearchArgs() {
    }
    #endregion

    #region Methods
    private HttpWebRequest CreateRequest<T>() {
      var sb = new System.Text.StringBuilder(ReportUri);
      sb.Append("requestType=retrieve&format=xml");

      sb.AppendFormat("&dataSource={0}s", typeof(T).Name.ToLower());

      switch (MostRecent) {
        case MostRecentType.Any:
          sb.AppendFormat("&mostRecent={0}", true);
          break;
        case MostRecentType.Each:
          sb.AppendFormat("&mostRecentForEachStation={0}", true);
          break;
      }

      if (HoursBeforeNow.HasValue)
        sb.AppendFormat("&hoursBeforeNow={0}", HoursBeforeNow);

      if (EndTime.HasValue)
        sb.AppendFormat("&endTime={0:O}", EndTime);

      if (StartTime.HasValue)
        sb.AppendFormat("&startTime={0:O}", StartTime);

      if ((Stations != null) && (Stations.Length != 0))
        sb.AppendFormat("&stationString={0}", System.Web.HttpUtility.UrlEncode(string.Join(",", Stations)));

      return (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
    }

    /// <summary>
    /// Searches the AWC website using the current search parameters.
    /// </summary>
    /// <returns></returns>
    public Response<T> Search<T>() {
      using (var resp = (HttpWebResponse)CreateRequest<T>().GetResponse()) {
        using (var stream = resp.GetResponseStream()) {
          return Response<T>.FromStream(stream);
        }
      }
    }
    #endregion

    #region Inner classes
    /// <summary>
    /// Specifies the querystring name for a property
    /// </summary>
    [AttributeUsage( AttributeTargets.Property)]
    public class QueryStringAttribute : Attribute {
      /// <summary>
      /// Gets or sets the property name of the query string.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Creates an instance of the QueryAttribute class.
      /// </summary>
      public QueryStringAttribute(string name) {
        Name = name;
      }
    }
    #endregion
  }
}
