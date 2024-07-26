using System.ComponentModel.DataAnnotations;

namespace Phat_Blogger_Website.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int MainCommentId { get; set; }
        [Required]
        public string Message { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? Avatar { get; set; }
    }
}
