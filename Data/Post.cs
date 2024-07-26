namespace Phat_Blogger_Website.Data
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        //thêm thuộc tính Image chứa đường dẫn ảnh
        //Add migration và update database để thay đổi csdl
        public string Image { get; set; } = "";

        //Thêm các thuộc tính miêu tả loại post
        //Tiếp tục Add Migration để cập nhật csdl 
        public string Description { get; set; } = "";
        public string Tags { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public List<MainComment> MainComments { get; set; }

        public Post()
        {
            MainComments = new List<MainComment>();
        }
    }
}
