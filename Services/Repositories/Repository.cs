using Microsoft.EntityFrameworkCore;
using Phat_Blogger_Website.Data;
using Phat_Blogger_Website.Helpers;
using Phat_Blogger_Website.ViewModels;
using System.Security.Claims;

namespace Phat_Blogger_Website.Services.Repositories
{
    public class Repository : IRepository
    {
        private AppDbContext _ctx;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Repository(AppDbContext ctx, IHttpContextAccessor httpContextAccessor)
        {
            _ctx = ctx;
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }

        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

        public IndexViewModel GetAllPosts(int pageNumber, string category, string search)
        {
            
            Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };

            int pageSize = 3;
            int skipAmount = pageSize * (pageNumber - 1);

            var query = _ctx.Posts.Include(mc => mc.MainComments).AsNoTracking().AsQueryable();
            
            if (!String.IsNullOrEmpty(category))
            {
                query = query.Where(x => x.Category.Equals(category));
            }

            if (!String.IsNullOrEmpty(search))
            {
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{search}%")
                || EF.Functions.Like(x.Body, $"%{search}%")
                || EF.Functions.Like(x.Description, $"%{search}%"));
            }

            int postCount = query.Count();
            int pageCount = (int)Math.Ceiling((double)postCount / pageSize);

            return new IndexViewModel
            {
                PageNumber = pageNumber,
                PageCount = pageCount,
                NextPage = postCount > skipAmount + pageSize,
                Pages = PageHelper.PageNumbers(pageNumber, pageCount).ToList(),
                Category = category,
                Search = search,
                Posts = query
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToList()
            };
        }

        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.MainComments)
                   .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.Id == id);
        }

        public PostViewModel GetPostDetail(int id)
        {
            var post = _ctx.Posts
                .Include(p => p.MainComments)
                   .ThenInclude(u => u.User)
                .Include(p => p.MainComments)
                   .ThenInclude(mc => mc.SubComments)
                       .ThenInclude(u => u.User)
                .SingleOrDefault(p => p.Id == id);

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var result = new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                Description = post.Description,
                Tags = post.Tags,
                Category = post.Category,
                MainComments = post.MainComments,
                Created = post.Created,
                ImageURL = post.Image,
                UserId = userId,
                UserName = userName
            };

            return result;
        }

        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }
        public void UpdatePost(Post post)
        {
            _ctx.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
    }
}
