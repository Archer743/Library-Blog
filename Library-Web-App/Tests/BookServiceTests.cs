using Library_Web_App.Data;
using Library_Web_App.Service.Interfaces;
using Library_Web_App.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;

namespace Tests
{
    public class BookServiceTests
    {
        private ApplicationContext context;
        private readonly Mock<ILikeService> likeServiceMock = new Mock<ILikeService>();
        private readonly Mock<ICommentService> commentServiceMock = new Mock<ICommentService>();
        private BookService bookService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TestDB").Options;

            context = new ApplicationContext(options);

            bookService = new BookService(context, likeServiceMock.Object, commentServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
        }

        [Test]
        public void GeByIdShouldReturnBook()
        {
            const int id = 1;
            Book book = AddTestBook(id);

            Book resultBook = bookService.GetById(id);

            Assert.AreEqual(resultBook != null, true);
            Assert.AreEqual(AreBooksEqual(book, resultBook), true);
        }

        [Test]
        public void GetByIdShouldReturnNullIfBookNotFound()
        {
            const int id = -1;

            Book resultBook = bookService.GetById(id);

            Assert.AreEqual(resultBook == null, true);
        }

        [Test]
        public void AddShouldAddNewBook()
        {
            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);

            Assert.AreEqual(context.Books.ToList().Count() == 1, true);
        }

        [Test]
        public void EditShouldUpdateBook()
        {
            const int id = 1;

            Book book = AddTestBook(id);

            List<Book> books = context.Books.ToList();
            Assert.True(books.Count() == 1);
            Assert.True(books[0].Id == id && books[0].Title == "title");

            book.Title = "title2";
            bookService.Edit(book);

            books = context.Books.ToList();
            Assert.True(books.Count() == 1);
            Assert.True(books[0].Id == id && books[0].Title == "title2");
        }

        [Test]
        public void DeleteShouldNotRemoveBookIfItNotFound()
        {
            const int id = -1;

            Book book = new Book(10, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);

            Assert.True(context.Books.ToList().Count() == 1);

            bookService.Delete(id);

            Assert.True(context.Books.ToList().Count() == 1);
        }

        [Test]
        public void DeleteShouldRemoveBook()
        {
            const int id = 1;

            AddTestBook(id);

            Assert.True(context.Books.ToList().Count() == 1);

            bookService.Delete(id);

            Assert.True(context.Books.ToList().Count() == 0);
        }

        [Test]
        public void GetAllShouldReturnAllBooks()
        {
            const int id = 1;
            const int id1 = 2;
            const int id2 = 3;

            AddTestBook(id);
            AddTestBook(id1);
            AddTestBook(id2);

            Assert.AreEqual(context.Books.ToList().Count() == 3, true);

            List<BookIndexViewModel> booksIndexView = bookService.GetAll();
            Assert.AreEqual(booksIndexView.Count() == 3, true);
        }

        [Test]
        public void GetByIdExtendedInfoShouldReturnNullIfBookNotFound()
        {
            const int id = -1;
            string curUserName = "";

            Assert.AreEqual(bookService.GetByIdExtendedInfo(curUserName, id) == null, true);
        }

        [Test]
        public void GetByIdExtendedInfoShouldReturnBookFullInfo()
        {
            const string userName = "Duner";

            const int id = 1;
            const int likesCount = 5;
            const bool likedByCurUser = true;

            Book book = AddTestBook(id);

            Assert.True(context.Books.ToList().Count() == 1);


            likeServiceMock.Setup(x => x.GetLikesCount(id))
                            .Returns(likesCount);

            likeServiceMock.Setup(x => x.IsBookLikedByCurUser(id, userName))
                           .Returns(likedByCurUser);

            BookInfoViewModel bookInfo = bookService.GetByIdExtendedInfo(userName, id);
            Book resultBook = bookInfo.Book;

            Assert.AreEqual(AreBooksEqual(resultBook, book), true);
            Assert.AreEqual(bookInfo.Likes == likesCount, true);
            Assert.AreEqual(bookInfo.LikedByCurUser == likedByCurUser, true);
        }

        public Book AddTestBook(int id)
        {
            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            return book;
        }

        public bool AreBooksEqual(Book one, Book two)
            =>  one?.Id == two?.Id &&
                one?.Title == two?.Title &&
                one?.Preview == two?.Preview &&
                one?.Link == two?.Link &&
                one?.Genre == two?.Genre &&
                one?.Author == two?.Author &&
                one?.Pages == two?.Pages &&
                one?.YearOfPublication == two?.YearOfPublication;
    }
}