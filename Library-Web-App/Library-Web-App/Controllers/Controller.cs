using Microsoft.AspNetCore.Mvc;
using ControllerClass = Microsoft.AspNetCore.Mvc.Controller;

namespace Library_Web_App.Controllers
{
    public class Controller : ControllerClass
    {
        public IActionResult NotAuthenticated()
            => View();

        public IActionResult UnauthorizedAccess()
            => View();

        public IActionResult CheckUserAuthenticationAndAdminRole()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            if (!User.IsInRole("admin"))
                return RedirectToAction(nameof(UnauthorizedAccess));

            return null;
        }
    }
}
