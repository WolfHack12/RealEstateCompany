using System;

namespace Exceptions
{
    public class ConsoleMenuException : Exception
    {
        public ConsoleMenuException(string message)
            : base(message)
        { }
    }
}
