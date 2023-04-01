using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library_Web_App.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                List<BookIndexViewModel> books = bookService.GetAll();
                return View(books);
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Create()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return View();
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                bookService.Add(book);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Edit(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                Book book = bookService.GetById(id);
                return View(book);
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                bookService.Edit(book);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Delete(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                Book book = bookService.GetById(id);
                return View(book);
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult DeleteConfirmed(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                bookService.Delete(id);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Info(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                BookInfoViewModel book = bookService.GetByIdExtendedInfo(User.Identity.Name, id);
                return View(book);
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Like(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                bookService.Like(id, User.Identity.Name);
                return RedirectToAction(nameof(Info), new { id = id });
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Dislike(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                bookService.Dislike(id, User.Identity.Name);
                return RedirectToAction(nameof(Info), new { id = id });
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult Comment(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return View(id);
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        [HttpPost]
        public IActionResult Comment(int id, string message)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                bookService.AddComment(id, User.Identity.Name, message);
                return RedirectToAction(nameof(Info), new { id = id });
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult DeleteComment(int id)
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                int bookId = bookService.DeleteCommentById(id);

                if (bookId != -1)
                    return RedirectToAction(nameof(Info), new { id = bookId });

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(NotAuthenticated));
        }

        public IActionResult NotAuthenticated()
            => View();
    }
}
