using System;
using System.Runtime.Serialization;

namespace eu.bayly.EADBasicNet.EAD {
  /// <summary>
  /// Represents errors that occur when querying the EAD website.
  /// </summary>
  public class EADException : Exception {
    /// <summary>The System.Runtime.Serialization.StreamingContext that contains contextual
    /// information about the source or destination.
    /// Initializes a new instance of the System.Exception class.
    /// </summary>
    public EADException() : base() { }

    /// <summary>
    /// Initializes a new instance of the System.Exception class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EADException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the System.Exception class with serialized data.
    /// </summary>
    /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
    public EADException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <summary>
    ///    Initializes a new instance of the System.Exception class with a specified
    ///     error message and a reference to the inner exception that is the cause of
    ///     this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException"></param>
    public EADException(string message, Exception innerException) : base(message, innerException) { }
  }
}