namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Exception thrown when an HTTP scoped service provider is requested but no HTTP context is available.
/// </summary>
public class HttpScopeNotAvailableException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="HttpScopeNotAvailableException"/>.
    /// </summary>
    public HttpScopeNotAvailableException() : base("No HTTP scope is currently available.")
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="HttpScopeNotAvailableException"/> with a specified message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public HttpScopeNotAvailableException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="HttpScopeNotAvailableException"/> with a specified message and inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public HttpScopeNotAvailableException(string message, Exception? innerException) : base(message, innerException)
    {
    }
}