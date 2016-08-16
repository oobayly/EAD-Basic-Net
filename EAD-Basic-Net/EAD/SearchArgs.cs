using System;
using System.Collections.Generic;
using System.Linq;

namespace eu.bayly.EADBasicNet.EAD {
  /// <summary>
  /// Represents EAD search arguments.
  /// </summary>
  public class SearchArgs {
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
    public static List<Document> Search(string session, int id) {
      var list = new List<Document>();

      return list;
    }
    #endregion
  }
}
