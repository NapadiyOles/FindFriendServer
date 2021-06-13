using System.Collections.Generic;

namespace FindFriend.Data.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public IEnumerable<Add> Adds { get; set; }
        public IEnumerable<Add> Favourites { get; set; }
    }
}