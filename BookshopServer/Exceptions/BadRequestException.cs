namespace BookShopServer.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a bad request is made to the application,
/// typically due to invalid input or client-side errors.
/// </summary>
public class BadRequestException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <c>BadRequestException</c> class
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public BadRequestException(string message) : base(message)
    {
    }
}