using Library_Web_App.Data.Entities;
using Library_Web_App.Service;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_App.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Index()
        {
            List<User> users = userService.GetAll();
            return View(users);
        }
    }
}
