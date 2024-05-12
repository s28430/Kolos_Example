namespace LeakedKolos.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException()
    {
    }

    public BookNotFoundException(string? message) : base(message)
    {
    }
}