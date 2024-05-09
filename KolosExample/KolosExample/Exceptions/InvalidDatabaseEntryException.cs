namespace KolosExample.Exceptions;

public class InvalidDatabaseEntryException : Exception
{
    public InvalidDatabaseEntryException()
    {
    }

    public InvalidDatabaseEntryException(string? message) : base(message)
    {
    }
}