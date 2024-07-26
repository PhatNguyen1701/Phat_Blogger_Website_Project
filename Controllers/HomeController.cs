using Microsoft.AspNetCore.Mvc;
using Phat_Blogger_Website.Data;
using Phat_Blogger_Website.Helpers;
using Phat_Blogger_Website.Models;
using Phat_Blogger_Website.Services.FileManager;
using Phat_Blogger_Website.Services.Repositories;
using Phat_Blogger_Website.ViewModels;
using System.Diagnostics;

namespace Phat_Blogger_Website.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;
        private ISendEmail _emailSender;

        public HomeController(IRepository repo, IFileManager fileManager, ISendEmail emailSender)
        {
            _repo = repo;
            _fileManager = fileManager;
            _emailSender = emailSender;
            var comment = new MainComment();
        }

        public IActionResult Index(int pageNumber, string category, string search)
        {
            if (pageNumber < 1)
                return RedirectToAction("Index", new { pageNumber = 1, category });

            var vm = _repo.GetAllPosts(pageNumber, category, search);

            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(EmailSenderViewModel vm)
        {
            await _emailSender.SendEmailAsync(vm.Email, vm.Subject, vm.Message);
            return View(vm);
        }

        public IActionResult Gallery()
        {
            var vm = _repo.GetAllPosts();
            return View(vm);
        }

        public IActionResult Post(int id)
        {
            var post = _repo.GetPostDetail(id);
            return View(post);
        }

        [HttpGet("/Image/{image}")]
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult Image(string image)
        {
            var mine = image.Substring(image.LastIndexOf("."));
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mine}");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = vm.PostId });
            }

            var post = _repo.GetPost(vm.PostId);
            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    UserId = vm.UserId,
                    Message = vm.Message,
                    Created = DateTime.Now
                });

                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    UserId = vm.UserId,
                    Message = vm.Message,
                    Created = DateTime.Now
                };
                _repo.AddSubComment(comment);
            }

            await _repo.SaveChangesAsync();
            return RedirectToAction("Post", new { id = vm.PostId });
        }
    }
}
