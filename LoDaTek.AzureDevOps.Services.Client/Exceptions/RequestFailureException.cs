
namespace LoDaTek.AzureDevOps.Services.Client.Exceptions;

/// <summary>
/// Request Failure Exception.
/// Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class RequestFailureException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestFailureException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RequestFailureException(string message) : base(message)
    {

    }
}
