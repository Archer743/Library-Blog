using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.User;
using Library_Web_App.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly ILikeService likeService;

        public UserController(IUserService userService, ILikeService likeService)
        {
            this.userService = userService;
            this.likeService = likeService;
        }

        public IActionResult Index()
        {
            IActionResult errorAction = CheckUserAuthenticationAndAdminRole();

            if (errorAction != null)
                return errorAction;

            List<UserIndexViewModel> users = userService.GetAllExtended();
            return View(users);
        }

        public IActionResult LikedBooks()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            List<BookIndexViewModel> likedBooks = userService.GetLikedBookes(User.Identity.Name);
            return View(likedBooks);
        }

        public IActionResult Dislike(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            likeService.Dislike(id, User.Identity.Name);
            return RedirectToAction(nameof(LikedBooks));
        }
    }
}
