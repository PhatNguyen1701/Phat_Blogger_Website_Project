using Phat_Blogger_Website.Data;
using Phat_Blogger_Website.ViewModels;

namespace Phat_Blogger_Website.Services.Repositories
{
    public interface IRepository
    {
        Post GetPost(int id);
        PostViewModel GetPostDetail(int id);
        List<Post> GetAllPosts();
        IndexViewModel GetAllPosts(int pageNumber, string category, string search);
        void AddPost(Post post);
        void RemovePost(int id);
        void UpdatePost(Post post);
        void AddSubComment(SubComment comment);

        Task<bool> SaveChangesAsync();
    }
}
