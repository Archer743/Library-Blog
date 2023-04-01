using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.User;
using Library_Web_App.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IBookService bookService;


        public UserController(IUserService userService, IBookService bookService)
        {
            this.userService = userService;
            this.bookService = bookService;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                List<UserIndexViewModel> users = userService.GetAllExtended();
                return View(users);
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult LikedBooks()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                List<BookIndexViewModel> likedBooks = userService.GetLikedBookes(User.Identity.Name);
                return View(likedBooks);
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Dislike(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                bookService.Dislike(id, User.Identity.Name);
                return RedirectToAction(nameof(LikedBooks));
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult NotAuthenticated()
            => View();
    }
}
