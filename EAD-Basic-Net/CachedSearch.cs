using eu.bayly.EADBasicNet.EAD;
using Newtonsoft.Json;
using System;
using System.IO;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// Represents an object used for storing cached search results.
  /// </summary>
  public class CachedSearch {
    /// <summary>
    /// Gets or sets the documents returned by the search.
    /// </summary>
    public Document[] Documents { get; set; }

    /// <summary>
    /// Gets whether the cached search results are valid.
    /// </summary>
    public bool IsValid {
      get {
        return DateTime.UtcNow < Timestamp.AddDays(1);
      }
    }

    /// <summary>
    /// Gets or sets the MD5 sum of the SearchArgs used.
    /// </summary>
    public string MD5Sum { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of when the documents were valid.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Reads the CachedSearch from the specified file.
    /// </summary>
    public static CachedSearch FromFile(FileInfo file) {
      using (var stream = file.OpenRead()) {
        using (var reader = new StreamReader(stream)) {
          return JsonConvert.DeserializeObject<CachedSearch>(reader.ReadToEnd());
        }
      }
    }

    /// <summary>
    /// Writes the CachedSearch to the specified file.
    /// </summary>
    public void Write(FileInfo file) {
      using (var stream = file.OpenWrite()) {
        using (var writer = new StreamWriter(stream)) {
          writer.Write(JsonConvert.SerializeObject(this));
        }
      }
      file.LastWriteTime = this.Timestamp;
    }
  }
}
