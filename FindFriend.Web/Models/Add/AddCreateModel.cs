using Microsoft.AspNetCore.Http;

namespace FindFriend.Web.Models.Add
{
    public class AddCreateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Phone { get; set; }
        public IFormFile Picture { get; set; }
    }
}   