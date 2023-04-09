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
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            List<BookIndexViewModel> books = bookService.GetAll();
            return View(books);
        }

        public IActionResult Create()
        {
            IActionResult errorAction = CheckUserAuthenticationAndAdminRole();
            
            if (errorAction != null)
                return errorAction;

            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            IActionResult errorAction = CheckUserAuthenticationAndAdminRole();

            if (errorAction != null)
                return errorAction;

            bookService.Add(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            IActionResult errorAction = CheckUserAuthenticationAndAdminRole();

            if (errorAction != null)
                return errorAction;

            Book book = bookService.GetById(id);
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            IActionResult errorAction = CheckUserAuthenticationAndAdminRole();

            if (errorAction != null)
                return errorAction;

            bookService.Edit(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            IActionResult errorAction = CheckUserAuthenticationAndAdminRole();

            if (errorAction != null)
                return errorAction;

            Book book = bookService.GetById(id);
            return View(book);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            IActionResult errorAction = CheckUserAuthenticationAndAdminRole();

            if (errorAction != null)
                return errorAction;

            bookService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Info(int id)
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction(nameof(NotAuthenticated));

            BookInfoViewModel book = bookService.GetByIdExtendedInfo(User.Identity.Name, id);
            return View(book);
        }
    }
}
