using System.Collections.Generic;
using FindFriend.Web.Models.Add;

namespace FindFriend.Web.Models.User
{
    public class UserFullViewModel : UserViewModel
    {
        public IEnumerable<AddViewModel> Authored { get; set; }
        public IEnumerable<AddViewModel> Liked { get; set; }
    }
}