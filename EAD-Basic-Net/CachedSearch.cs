using eu.bayly.EADBasicNet.EAD;
using Newtonsoft.Json;
using System;
using System.IO;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// Represents an object used for storing cached search results.
  /// </summary>
  public class CachedSearch<T> {
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
    /// Gets or sets the value.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Reads the CachedSearch from the specified file.
    /// </summary>
    public static CachedSearch<T> FromFile(FileInfo file) {
      using (var stream = file.OpenRead()) {
        using (var reader = new StreamReader(stream)) {
          return JsonConvert.DeserializeObject<CachedSearch<T>>(reader.ReadToEnd());
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
