using System;

namespace FindFriend.Business.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message)
        {
        }
    }
}