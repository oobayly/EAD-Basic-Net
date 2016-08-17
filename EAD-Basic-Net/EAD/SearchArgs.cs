using System;
using System.Collections.Generic;
using System.Net;

namespace eu.bayly.EADBasicNet.EAD {
  /// <summary>
  /// Represents EAD search arguments.
  /// </summary>
  public class SearchArgs : EADBase {
    #region Properties
    /// <summary>
    /// Gets or sets which AIRAC information to search for.
    /// </summary>
    public AIRAC AIRAC { get; set; }

    /// <summary>
    /// Gets or sets the authority to search for.
    /// </summary>
    public Authority Authority { get; set; }

    /// <summary>
    /// Gets or sets the type of authority to search for.
    /// </summary>
    public AuthorityType AuthorityType { get; set; }

    /// <summary>
    /// Gets or sets the effective date to search for.
    /// </summary>
    public DateTime? Effective { get; set; }

    /// <summary>
    /// Gets or sets the language to search for.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets the name of the document to search for.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the part-AIRAC information to search for.
    /// </summary>
    public PartAIRAC PartAIRAC { get; set; }

    /// <summary>
    /// Gets or sets the title of the document to search for.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the type of document to search for.
    /// </summary>
    public DocumentType Type { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates an instance of the SearchArgs class.
    /// </summary>
    public SearchArgs() {
      this.AIRAC = EAD.AIRAC.Any;
      this.Authority= Authority.EG;
      this.AuthorityType = EAD.AuthorityType.Any;
      this.Effective = null;
      this.Language = EAD.Language.Any;
      this.Name = null;
      this.PartAIRAC = EAD.PartAIRAC.Any;
      this.Title = null;
      this.Type = DocumentType.Any;
    }
    #endregion

    #region Methods
    private HttpWebRequest CreateRequest(int page = 0) {
      var sb = new System.Text.StringBuilder(SearchUri + Session);
      sb.AppendFormat("?authority={0}", Authority);
      // Authority type, first char only
      sb.AppendFormat("&authoritytype={0}", AuthorityType == AuthorityType.Any ? "***" : AuthorityType.ToString().Substring(0, 1));
      // Language (optional)
      sb.AppendFormat("&lang={0}", (Language == Language.Any ? "***" : Language.ToString()));
      // Type (optional)
      sb.AppendFormat("&type={0}", (Type == DocumentType.Any ? "***" : Type.ToString()));
      // PartAIRAC (optional)
      sb.AppendFormat("&partairac={0}", (PartAIRAC == PartAIRAC.Any ? "***" : PartAIRAC.ToString()));
      // Airac (optional)
      sb.AppendFormat("&part={0}", (AIRAC == EAD.AIRAC.Any ? "***" : AIRAC.ToString()));
      sb.AppendFormat("&airac={0}", (AIRAC == EAD.AIRAC.Any ? "***" : AIRAC.ToString()));
      // Date
      sb.AppendFormat("&effectiveyear={0}", Effective.HasValue ? (int?)Effective.Value.Year : null);
      sb.AppendFormat("&effectivemonth={0}", Effective.HasValue ? (int?)Effective.Value.Month : null);
      sb.AppendFormat("&effectiveday={0}", Effective.HasValue ? (int?)Effective.Value.Day : null);
      // Blank
      sb.Append("&org=");
      // Document name
      sb.AppendFormat("&docname={0}", System.Web.HttpUtility.UrlEncode(Name));
      // Document title
      sb.AppendFormat("&docheading={0}", System.Web.HttpUtility.UrlEncode(Title));
      // Form constants
      sb.Append("&option=search");
      sb.Append("&advancedCheckBox=on");
      sb.AppendFormat("&id={0}", ID);
      sb.AppendFormat("&index={0}", page);

      return (HttpWebRequest)HttpWebRequest.Create(sb.ToString());
    }

    /// <summary>
    /// Searches the EAD website using the current search parameters.
    /// </summary>
    /// <exception cref="EADException">The EAD website returned an error.</exception>
    public Document[] Search() {
      // Authenticate only if session expired
      if (HasExpired && !Authenticate()) {
        throw new WebException("Not authenticated.");
      }

      var list = new List<Document>();
      int page = 0;
      int pageCount;
      string text = null;
      while (true) {
        if (MakeRequest(CreateRequest(page), out text)) {
          list.AddRange(Document.FromHtml(text, out pageCount));
          if (page < (pageCount - 1)) {
            page++;
          } else {
            break;
          }
        }
      }

      list.Sort();

      return list.ToArray();
    }
    #endregion
  }
}
