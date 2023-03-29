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
            List<Book> books = bookService.GetAll();
            return View(books);
        }

        public IActionResult Info(int id)
        {
            /*if (User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(Index));*/

            InfoViewModel book = bookService.GetByIdWithLikesAndUserLikeBoolean(User.Identity.Name, id);
            return View(book);
        }

        public IActionResult Like(int id)
        {
            bookService.Like(id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Dislike(int id)
        {
            bookService.Dislike(id, User.Identity.Name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
            => View();

        [HttpPost]
        public IActionResult Create(Book book)
        {
            bookService.Add(book);
            return RedirectToAction(nameof(Index));
        }

        /*public IActionResult Edit(int id)
        {
            Book book = bookService.GetById(id);
            return View(book);
        }*/

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
    }
}
