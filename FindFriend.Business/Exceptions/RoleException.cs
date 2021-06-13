using System;

namespace FindFriend.Business.Exceptions
{
    public class RoleException : Exception
    {
        public RoleException(string message) : base(message)
        {
        }
    }
}