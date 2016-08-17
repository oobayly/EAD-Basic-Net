using System;

namespace eu.bayly.EADBasicNet {
  /// <summary>
  /// A wrapper to allow literals to be posted.
  /// </summary>
  public class Request<T> {
    /// <summary>
    /// Gets or sets the request's value.
    /// </summary>
    public T Value { get; set; }
  }
}