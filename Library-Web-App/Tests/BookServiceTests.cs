using Library_Web_App.Data;
using Library_Web_App.Service.Interfaces;
using Library_Web_App.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using Library_Web_App.Data.Entities;
using static System.Reflection.Metadata.BlobBuilder;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.Comment;

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

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            Book resultBook = bookService.GetById(id);

            Assert.True(resultBook != null);
            Assert.True(resultBook?.Id == id);
        }

        [Test]
        public void GeByIdShouldReturnNullIfBookNotFound()
        {
            const int id = -69;

            Assert.True(bookService.GetById(id) == null);
        }

        [Test]
        public void AddShouldAddNewBook()
        {
            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);

            Assert.True(context.Books.ToList().Count() != 0);
        }

        [Test]
        public void EditShouldUpdateBook()
        {
            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);

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
            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);

            Assert.True(context.Books.ToList().Count() == 1);

            bookService.Delete(-id);

            Assert.True(context.Books.ToList().Count() == 1);
        }

        [Test]
        public void DeleteShouldRemoveBook()
        {
            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);

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

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            Book book1 = new Book(id1, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            Book book2 = new Book(id2, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);
            bookService.Add(book1);
            bookService.Add(book2);

            Assert.True(context.Books.ToList().Count() == 3);

            List<BookIndexViewModel> booksIndexView = bookService.GetAll();
            Assert.True(booksIndexView.Count() == 3);
        }

        [Test]
        public void GetByIdExtendedInfoShouldReturnNullIfBookNotFound()
        {
            const int id = -69;
            string curUserName = "";

            Assert.True(bookService.GetByIdExtendedInfo(curUserName, id) == null);
        }

        [Test]
        public void GetByIdExtendedInfoShouldReturnBookFullInfo()
        {
            string userName = "Duner";

            const int id = 1;
            const int id1 = 2;
            const int id2 = 3;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            Book book1 = new Book(id1, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            Book book2 = new Book(id2, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            bookService.Add(book);
            bookService.Add(book1);
            bookService.Add(book2);

            Assert.True(context.Books.ToList().Count() == 3);



            likeServiceMock.Setup(x => x.GetLikesCount(id))
                            .Returns(5);

            commentServiceMock.Setup(x => x.GetCommentsExtendedInfo(id))
                           .Returns(new List<CommentInfoViewModel>());

            likeServiceMock.Setup(x => x.IsBookLikedByCurUser(id, userName))
                           .Returns(true);

            Assert.True(bookService.GetByIdExtendedInfo(userName, id).Book.Id == id);



            likeServiceMock.Setup(x => x.GetLikesCount(id1))
                            .Returns(6);

            commentServiceMock.Setup(x => x.GetCommentsExtendedInfo(id1))
                           .Returns(new List<CommentInfoViewModel>());

            likeServiceMock.Setup(x => x.IsBookLikedByCurUser(id1, userName))
                           .Returns(false);

            Assert.True(bookService.GetByIdExtendedInfo(userName, id1).Book.Id == id1);



            likeServiceMock.Setup(x => x.GetLikesCount(id2))
                            .Returns(7);

            commentServiceMock.Setup(x => x.GetCommentsExtendedInfo(id2))
                           .Returns(new List<CommentInfoViewModel>());

            likeServiceMock.Setup(x => x.IsBookLikedByCurUser(id2, userName))
                           .Returns(true);

            Assert.True(bookService.GetByIdExtendedInfo(userName, id2).Book.Id == id2);

        }
    }
}
