using System;

namespace Exceptions
{
    public class OverflowEstatesCollectionException : Exception
    {
        public OverflowEstatesCollectionException(string message)
            : base(message)
        { }
    }
}
