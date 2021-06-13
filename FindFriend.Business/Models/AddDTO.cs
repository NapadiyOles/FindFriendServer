using System.Drawing;

namespace FindFriend.Business.Models
{
    public class AddDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Phone { get; set; }
        public Image Picture { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}