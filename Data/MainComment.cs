namespace Phat_Blogger_Website.Data
{
    public class MainComment
    {
        public int MainCommentId { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public List<SubComment> SubComments { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public MainComment()
        {
            SubComments = new List<SubComment>();
        }
    }
}
