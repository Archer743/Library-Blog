using Library_Web_App.Data.Entities;
using Library_Web_App.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Library_Web_App.Controllers
{
    public class BookController : Controller
    {
        private BookService bookService;

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
            Book book = bookService.GetById(id);
            return View(book);
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

        public IActionResult DeleteConfirmed(Book toBeDeleted) // да работи с id
        {
            bookService.Delete(toBeDeleted);
            return RedirectToAction(nameof(Index));
        }
    }
}
