using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class UnexistantUserException : Exception
    {
        public UnexistantUserException(string message)
            : base(message)
        { }
    }
}
