using System.Reflection.PortableExecutable;
using FindFriend.Web.Models.User;

namespace FindFriend.Web.Models.Add
{
    public class AddFullViewModel : AddViewModel
    {
        public string Description { get; set; }
        public string Phone { get; set; }
        public AuthorViewModel Author { get; set; }
    }
}