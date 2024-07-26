using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phat_Blogger_Website.Data;
using Phat_Blogger_Website.Services.FileManager;
using Phat_Blogger_Website.ViewModels;

namespace Phat_Blogger_Website.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private IFileManager _fileManager;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, IFileManager fileManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileManager = fileManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            var result = await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            if (!result.Succeeded)
            {
                return View(vm);
            }

            var user = await _userManager.FindByNameAsync(vm.UserName);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
                return RedirectToAction("Index", "Panel");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = new User
            {
                UserName = vm.Email,
                Email = vm.Email,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Address = vm.Address,
                DoB = vm.DoB,
                Gender = vm.Gender,
                PhoneNumber = vm.PhoneNumber,
                Avatar = await _fileManager.SaveImage(vm.Avatar)
            };

            if (vm.Avatar == null)
            {
                user.Avatar = vm.CurrentImage;
            }
            else
            {
                if (!string.IsNullOrEmpty(vm.CurrentImage))
                    _fileManager.RemoveImage(vm.CurrentImage);

                user.Avatar = await _fileManager.SaveImage(vm.Avatar);
            }

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
