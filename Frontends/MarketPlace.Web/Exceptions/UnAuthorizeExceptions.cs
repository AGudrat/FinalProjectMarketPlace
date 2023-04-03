namespace MarketPlace.Web.Exceptions;

public class UnAuthorizeExceptions : Exception
{
    public UnAuthorizeExceptions()
    {
    }

    public UnAuthorizeExceptions(string? message) : base(message)
    {
    }

    public UnAuthorizeExceptions(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
