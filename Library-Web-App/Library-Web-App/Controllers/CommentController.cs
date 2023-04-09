using Library_Web_App.Data.ViewModels.Comment;
using Library_Web_App.Service;
using Library_Web_App.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_App.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        public IActionResult BookComments(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            BookCommentsViewModel bookComments = commentService.GetBookCommentsViewModel(id);
            return View(bookComments);
        }

        public IActionResult AddComment(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            return View(id);
        }

        [HttpPost]
        public IActionResult Comment(int id, string message)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            commentService.AddComment(id, User.Identity.Name, message);
            return RedirectToAction("Info", "Book", new { id = id });
        }

        public IActionResult DeleteComment(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            int bookId = commentService.DeleteCommentById(id, User.Identity.Name, User.IsInRole("admin"));

            if (bookId != -1)
                return RedirectToAction(nameof(BookComments), new { id = bookId });

            return RedirectToAction("Index", "Book");
        }
    }
}
