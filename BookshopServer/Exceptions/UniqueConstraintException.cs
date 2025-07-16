namespace BookShopServer.Exceptions;

public class UniqueConstraintException : Exception
{
    public UniqueConstraintException(string message) : base(message)
    {
    }
}