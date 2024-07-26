using System.ComponentModel.DataAnnotations;

namespace Phat_Blogger_Website.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool Gender { get; set; }

        [Required]
        public DateTime DoB { get; set; }

        public string? CurrentImage { get; set; } = "";

        public IFormFile Avatar { get; set; } = null;
    }
}
