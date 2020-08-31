using System;

namespace PostOffice.Exceptions
{
    public class UnexpectedLocationException : Exception
    {
        public UnexpectedLocationException(string message) : base(message)
        {
        }
    }
}