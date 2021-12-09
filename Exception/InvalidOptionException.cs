namespace Exceptions
{
    public class InvalidOptionException : ConsoleMenuException
    {
        public InvalidOptionException(string message)
            : base(message)
        { }
    }
}
