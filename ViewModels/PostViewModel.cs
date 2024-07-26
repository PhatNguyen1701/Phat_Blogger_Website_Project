using Phat_Blogger_Website.Data;

namespace Phat_Blogger_Website.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string Description { get; set; } = "";
        public string Tags { get; set; } = "";
        public string Category { get; set; } = "";
        public string CurrentImage { get; set; } = "";
        public IFormFile Image { get; set; } = null;
        public string ImageURL { get; set; }
        public ICollection<MainComment> MainComments { get; set; } = new List<MainComment>();
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
