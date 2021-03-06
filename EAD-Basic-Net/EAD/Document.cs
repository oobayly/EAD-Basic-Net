﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace eu.bayly.EADBasicNet.EAD {
  /// <summary>
  /// Represents an EAD document object.
  /// </summary>
  public class Document : IComparable<Document> {
    #region Fields
    private string name;

    /// <summary>
    /// The Unix Epoch
    /// </summary>
    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the Aeronautical Information Regulation And Control (AIRAC) information for the document.
    /// </summary>
    public AIRAC? AIRAC { get; set; }

    /// <summary>
    /// Gets or sets the effective date of the document.
    /// </summary>
    public DateTime? Effective { get; set; }

    /// <summary>
    /// Gets or sets the ICAO code of the document's airport (if applicable).
    /// </summary>
    public string ICAO { get; set; }

    /// <summary>
    /// Gets whether the document contains any data.
    /// </summary>
    public bool IsEmpty {
      get {
        return (AIRAC == null) && (Effective == null) && (Name == null) && (Uri == null) && (Title == null);
      }
    }

    /// <summary>
    /// Gets or sets the document's language.
    /// </summary>
    public Language? Language { get; set; }

    /// <summary>
    /// Gets or sets the name of the document.
    /// </summary>
    public string Name {
      get {
        return name;
      }
      set {
        name = value;
        OnNameChanged();
      }
    }

    /// <summary>
    /// Gets or sets the part-AIRAC type of the document.
    /// </summary>
    public PartAIRAC? PartAIRAC { get; set; }

    /// <summary>
    /// Gets or sets the string used for sorting.
    /// </summary>
    private string SortName { get; set; }

    /// <summary>
    /// Gets or sets the documents timestamp.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the title of the document.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets what type the document is.
    /// </summary>
    public DocumentType? Type { get; set; }

    /// <summary>
    /// Gets or sets the Uri to the document.
    /// </summary>
    public Uri Uri { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates an instance of the EAD document class.
    /// </summary>
    public Document() {
    }
    #endregion

    #region Methods
    /// <summary>
    /// Creates a list of documents from the specified HTHML response.
    /// </summary>
    /// <param name="html">The HTHML response from the EAD website.</param>
    /// <returns>The list of parsed documents.</returns>
    public static Document[] FromHtml(string html) {
      int pageCount;
      return FromHtml(html, out pageCount);
    }

    /// <summary>
    /// Creates a list of documents from the specified HTHML response.
    /// </summary>
    /// <param name="html">The HTHML response from the EAD website.</param>
    /// <param name="pageCount">The number of pages.</param>
    /// <returns>The list of parsed documents.</returns>
    public static Document[] FromHtml(string html, out int pageCount) {
      // Extract the base url
      var uriMatch = Regex.Match(html, "url.*?=.*?(?<uri>http[^\"]+)");

      var icao = new Regex(@"^[A-Z]{2}_[^_]+_[0-9]+_(?<ICAO>[A-Z]{4})", RegexOptions.Compiled);

      var whitespace = new Regex(@"\s\s+", RegexOptions.Compiled);

      var list = new List<Document>();
      pageCount = 0;

      DateTime timestamp = UnixEpoch.AddMilliseconds(EADBase.ID.Value);

      if (uriMatch.Success) {
        string baseUri = uriMatch.Groups["uri"].Value.Trim();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Get all the <tr> nodes containing the results
        HtmlNodeCollection rows = doc.DocumentNode.SelectNodes("/html/body/table[@class='list']/tr");

        foreach (HtmlNode row in rows) {
          var item = new Document() {
            Timestamp = timestamp
          };

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

            // Extract the ICAO code (if available)
            var icaoMatch = icao.Match(item.Name);
            if (icaoMatch.Success)
              item.ICAO = icaoMatch.Groups["ICAO"].Value;


            // URI is in a javascript href of the 4th node, wrapped
            // in html quotes
            string js = node.SelectSingleNode("a").Attributes["href"].Value.Trim();
            int first = js.IndexOf("&quot;");
            int last = js.IndexOf("&quot;", first + 1);
            js = js.Substring(first + 6, last - first - 6);
            item.Uri = new Uri(baseUri + "/" + js);

            // ENX7SHJ56S2BE/EN/Charts/AD/AIRAC/EI_AD_2_EIDW_24-10_en_2012-12-13.pdf
            var parts = js.Split('/');

            Language lang;
            if (Enum.TryParse<Language>(parts[1], out lang))
              item.Language = lang;

            DocumentType type;
            if (Enum.TryParse<DocumentType>(parts[2], out type))
              item.Type = type;

            PartAIRAC part;
            if (Enum.TryParse<PartAIRAC>(parts[3], out part))
              item.PartAIRAC = part;

            AIRAC airac;
            if (Enum.TryParse<AIRAC>(parts[4], out airac))
              item.AIRAC = airac;
          }

          // Document title
          node = row.SelectSingleNode("td[contains(@title, 'Show Document Information')]");
          if (node != null) {
            item.Title = whitespace.Replace(System.Web.HttpUtility.HtmlDecode(node.InnerText.Trim()), " ");
          }

          if (!item.IsEmpty)
            list.Add(item);
        }

        // Get number of pages
        var m = Regex.Match(html, @"^[\s]*pages[\s]*=[\s]*(?<Count>[0-9]+)", RegexOptions.Multiline);
        pageCount = m.Success ? int.Parse(m.Groups["Count"].Value) : 0;

      }

      return list.ToArray();
    }

    /// <summary>
    /// Compares the current instance with another document.
    /// </summary>
    public int CompareTo(Document obj) {
      return string.Compare(this.SortName, obj.SortName);
    }

    /// <summary>
    /// Raised when the name property is changed.
    /// </summary>
    protected void OnNameChanged() {
      if (name == null) {
        SortName = null;
        return;
      }

      /* Operate on the name - the last 3 chars (_XX)
       * Split the name into it's components, pad out any numeric values
       * to 5 chars, and recombine
       */
      string[] parts = name.Substring(0, name.Length - 3).Split('_', '-');
      StringBuilder sb = new StringBuilder();

      for (int i = 0; i < parts.Length; i++) {
        int parsed;
        if (int.TryParse(parts[i], out parsed)) {
          sb.AppendFormat("{0:00000}", parsed);
        } else {
          sb.Append(parts[i]);
        }
        sb.Append("_");
      }

      SortName = sb.ToString();
    }

    /// <summary>
    /// Returns a human readable representation of the document object.
    /// </summary>
    public override string ToString() {
      return "{Title: " + Title + "}";
    }
    #endregion
  }
}
