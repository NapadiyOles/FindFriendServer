using System.Collections.Generic;

namespace FindFriend.Data.Entities
{
    public class Add : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureName { get; set; }
        public string Phone { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public IEnumerable<User> Likers { get; set; }
    }
}