namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// Represents a response from the web service.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Response<T> {
    /// <summary>
    /// Gets or sets the error message attached to the response.
    /// </summary>
    public string Error { get; set; }

    /// <summary>
    /// Gets or sets the data attached to the response.
    /// </summary>
    public T Value { get; set; }
  }
}