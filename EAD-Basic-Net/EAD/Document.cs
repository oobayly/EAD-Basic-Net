using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace eu.bayly.EADBasicNet.EAD {
  /// <summary>
  /// Represents an EAD document object.
  /// </summary>
  public class Document : EADBase, IComparable<Document> {
    #region Properties
    /// <summary>
    /// Gets or sets the Aeronautical Information Regulation And Control (AIRAC) information for the document.
    /// </summary>
    public string AIRAC { get; set; }

    /// <summary>
    /// Gets or sets the effective date of the document.
    /// </summary>
    public DateTime? Effective { get; set; }

    /// <summary>
    /// Gets whether the document contains any data.
    /// </summary>
    public bool IsEmpty {
      get {
        return (AIRAC == null) && (Effective == null) && (Name == null) && (Uri == null) && (Title == null);
      }
    }

    /// <summary>
    /// Gets or sets the name of the document.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the Uri to the document.
    /// </summary>
    public Uri Uri { get; set; }

    /// <summary>
    /// Gets or sets the title of the document.
    /// </summary>
    public string Title { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates an instance of the EAD document class.
    /// </summary>
    public Document() {
    }
    #endregion

    #region Methods
    private static HttpWebRequest CreateRequest(SearchArgs args, int page = 0) {
      var sb = new System.Text.StringBuilder(SearchUri + Session);
      sb.AppendFormat("?authority={0}", args.Authority);
      // Authority type, first char only
      sb.AppendFormat("&authoritytype={0}", args.AuthorityType == AuthorityType.Any ? "***" : args.AuthorityType.ToString().Substring(0, 1));
       // Language (optional)
      sb.AppendFormat("&lang={0}", (args.Language == Language.Any ? "***" : args.Language.ToString()));
      // Type (optional)
      sb.AppendFormat("&type={0}", (args.Type == DocumentType.Any ? "***" : args.Type.ToString()));
      // PartAIRAC (optional)
      sb.AppendFormat("&partairac={0}", (args.PartAIRAC == PartAIRAC.Any ? "***" : args.PartAIRAC.ToString()));
      // Airac (optional)
      sb.AppendFormat("&part={0}", (args.AIRAC == EAD.AIRAC.Any ? "***" : args.AIRAC.ToString()));
      sb.AppendFormat("&airac={0}", (args.AIRAC == EAD.AIRAC.Any ? "***" : args.AIRAC.ToString()));
      // Date
      sb.AppendFormat("&effectiveyear={0}", args.Effective.HasValue ? (int?)args.Effective.Value.Year : null);
      sb.AppendFormat("&effectivemonth={0}", args.Effective.HasValue ? (int?)args.Effective.Value.Month : null);
      sb.AppendFormat("&effectiveday={0}", args.Effective.HasValue ? (int?)args.Effective.Value.Day : null);
      // Blank
      sb.Append("&org=");
      // Document name
      sb.AppendFormat("&docname={0}", System.Web.HttpUtility.UrlEncode(args.Name));
      // Document title
      sb.AppendFormat("&docheading={0}", System.Web.HttpUtility.UrlEncode(args.Title));
      // Form constants
      sb.Append("&option=search");
      sb.Append("&advancedCheckBox=on");
      sb.AppendFormat("&id={0}", ID);
      sb.AppendFormat("&index={0}", page);

      return (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
    }

    /// <summary>
    /// Parses the html response.
    /// </summary>
    /// <param name="html">The html response from the EAD website.</param>
    /// <param name="list">The list of document to populate.</param>
    /// <param name="page">The current page number.</param>
    /// <returns>A flag indicating if there are more pages.</returns>
    private static bool ParseResponse(string html, List<Document> list, int page) {
      // Extract the base url
      var uriMatch = Regex.Match(html, "url.*?=.*?(?<uri>http[^\"]+)");
      if (!uriMatch.Success)
        return false;
      string baseUri = uriMatch.Groups["uri"].Value.Trim();

      var doc = new HtmlDocument();
      doc.LoadHtml(html);

      // Get all the <tr> nodes containing the results
      HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("/html/body/table[@class='list']/tr");

      foreach (HtmlNode row in rows) {
        var item = new Document();

        HtmlNodeCollection cells = row.SelectNodes("td");
        HtmlNode node;

        // Date cell, stored as ms since Unix epoch
        node = row.SelectSingleNode("td[contains(@title, 'Effective Date')]");
        if (node != null) {
          DateTime effective = DateTime.ParseExact(
            node.InnerText.Trim(),
            "yyyy-MM-dd", null
            );
          item.Effective = DateTime.SpecifyKind(effective, DateTimeKind.Utc);

        }

        // PDF Name & URI cell
        node = row.SelectSingleNode("td[contains(@title, 'Open PDF Document')]");
        if (node != null) {
          item.Name = node.InnerText.Trim().Replace(".pdf", "");

          // URI is in a javascript href of the 4th node, wrapped
          // in html quotes
          string js = node.SelectSingleNode("a").Attributes["href"].Value.Trim();
          int first = js.IndexOf("&quot;");
          int last = js.IndexOf("&quot;", first + 1);
          item.Uri = new Uri(baseUri + "/" + js.Substring(first + 6, last - first - 6));
        }

        // AIRAC (optional)
        node = row.SelectSingleNode("td[contains(@title, 'AIRAC')]");
        if (node != null) {
          item.AIRAC = node.InnerText.Trim();
        }

        // Document title
        node = row.SelectSingleNode("td[contains(@title, 'Show Document Information')]");
        if (node != null) {
          item.Title = System.Web.HttpUtility.HtmlDecode(node.InnerText.Trim());
        }

        if (!item.IsEmpty)
          list.Add(item);
      }

      // Get number of pages
      var m = Regex.Match(html, @"^[\s]*pages[\s]*=[\s]*(?<Count>[0-9]+)", RegexOptions.Multiline);
      var pageCount = m.Success ? int.Parse(m.Groups["Count"].Value) : 0;

      return page < (pageCount - 1);
    }

    /// <summary>
    /// Searches the EAD website.
    /// </summary>
    public static List<Document> Search(SearchArgs args) {
      // Authenticate only if session expired
      if (HasExpired && !Authenticate()) {
        throw new WebException("Not authenticated");
      }

      var list = new List<Document>();
      int page = 0;
      string text = null;
      while (true) {
        if (MakeRequest(CreateRequest(args, page), out text)) {
          if (ParseResponse(text, list, page)) {
            page++;
          } else {
            break;
          }
        }
      }

      //list.Sort();

      return list;
    }

    /// <summary>
    /// Compares the current instance with another document.
    /// </summary>
    public int CompareTo(Document obj) {
      return string.Compare(this.Title, obj.Title);
    }
    #endregion
  }
}
