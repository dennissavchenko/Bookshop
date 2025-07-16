namespace BookShopServer.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a conflict occurs during the request processing,
/// such as attempting to create a duplicate resource or violating a uniqueness constraint.
/// </summary>
public class ConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <c>ConflictException</c> class.
    /// </summary>
    /// <param name="message">The error message that describes the conflict.</param>
    public ConflictException(string message) : base(message)
    {
    }
}
