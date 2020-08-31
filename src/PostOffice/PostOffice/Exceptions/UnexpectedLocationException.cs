using System;
using System.Collections.Generic;
using System.Linq;
using PostOffice.Models;

namespace PostOffice.Exceptions
{
    public class UnexpectedLocationException : Exception
    {
        public UnexpectedLocationException(string message) : base(message)
        {
        }
    }
}