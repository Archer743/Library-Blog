using Library_Web_App.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_App.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikeService likeService;

        public LikeController(ILikeService likeService)
        {
            this.likeService = likeService;
        }
        
        public IActionResult Like(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            likeService.Like(id, User.Identity.Name);
            return RedirectToAction("Info", "Book", new { id = id });
        }

        public IActionResult Dislike(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            likeService.Dislike(id, User.Identity.Name);
            return RedirectToAction("Info", "Book", new { id = id });
        }
    }
}
