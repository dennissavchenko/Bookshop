namespace BookShopServer.Exceptions;

/// <summary>
/// Represents an exception that is thrown when a requested resource cannot be found.
/// Typically used when an entity with a given identifier does not exist.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <c>NotFoundException</c> class.
    /// </summary>
    /// <param name="message">The error message that describes the missing resource.</param>
    public NotFoundException(string message) : base(message)
    {
    }
}
