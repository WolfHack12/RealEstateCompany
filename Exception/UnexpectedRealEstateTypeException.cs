using System;

namespace Exceptions
{
    public class UnexpectedRealEstateTypeException : Exception
    {
        public UnexpectedRealEstateTypeException(string message)
            : base(message)
        { }
    }
}
