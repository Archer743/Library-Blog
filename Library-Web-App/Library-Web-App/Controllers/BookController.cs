using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Library_Web_App.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService bookService;

        public BookController(BookService bookService)
        {
            this.bookService = bookService;
        }

        public IActionResult Index()
        {
            List<BookIndexViewModel> books = bookService.GetAll();
            return View(books);
        }

        public IActionResult Create()
            => View();

        [HttpPost]
        public IActionResult Create(Book book)
        {
            bookService.Add(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Book book = bookService.GetById(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            bookService.Edit(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Book book = bookService.GetById(id);
            return View(book);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            bookService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Info(int id)
        {
            /*if (User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(Index));*/

            BookInfoViewModel book = bookService.GetByIdExtendedInfo(User.Identity.Name, id);
            return View(book);
        }

        public IActionResult Like(int id)
        {
            bookService.Like(id, User.Identity.Name);
            return RedirectToAction(nameof(Info), new { id = id });
        }

        public IActionResult Dislike(int id)
        {
            bookService.Dislike(id, User.Identity.Name);
            return RedirectToAction(nameof(Info), new { id = id });
        }

        public IActionResult Comment(int id)
            => View(id);

        [HttpPost]
        public IActionResult Comment(int id, string message)
        {
            bookService.AddComment(id, User.Identity.Name, message);
            return RedirectToAction(nameof(Info), new { id = id });
        }

        public IActionResult DeleteComment(int id)
        {
            int bookId = bookService.DeleteCommentById(id);
            return RedirectToAction(nameof(Info), new { id = bookId });
        }
    }
}
