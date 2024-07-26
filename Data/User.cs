using Microsoft.AspNetCore.Identity;

namespace Phat_Blogger_Website.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Address { get; set; } = "";
        public bool Gender { get; set; }
        public DateTime DoB { get; set; }
        public string Avatar { get; set; } = "";
        public ICollection<MainComment> MainComments { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
    }
}
