using System;
using System.ComponentModel;
using System.Globalization;

namespace eu.bayly.EADBasicNet.OurAirports {
  /// <summary>
  /// Represents an attribute used for specifying a class's Uri.
  /// </summary>
  public class UriAttribute : Attribute {
    /// <summary>
    /// Gets or sets the attribute's Uri.
    /// </summary>
    public Uri Uri { get; set; }

    /// <summary>
    /// Creates an instance of the UriAttribute class.
    /// </summary>
    public UriAttribute(string uri) : this(new Uri(uri)) {
    }

    /// <summary>
    /// Creates an instance of the UriAttribute class.
    /// </summary>
    public UriAttribute(Uri uri) {
      this.Uri = uri;
    }
  }
}
