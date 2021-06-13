using System;

namespace FindFriend.Business.Exceptions
{
    public class AccessException : Exception
    {
        public AccessException(string message) : base(message)
        {
        }
    }
}