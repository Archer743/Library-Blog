using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.Comment;
using Library_Web_App.Service;
using Library_Web_App.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net;

namespace Tests
{
    public class CommentServiceTests
    {
        private ApplicationContext context;
        private Mock<IUserService> userServiceMock = new Mock<IUserService>();
        private CommentService commentService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase("TestDB").Options;

            context = new ApplicationContext(options);

            commentService = new CommentService(context, userServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            context.Database.EnsureDeleted();
        }

        [Test]
        public void BookExistsShouldReturnTrue()
        {
            const int id = 1;

            AddTestBook(id);

            Assert.AreEqual(commentService.BookExists(id), true);
        }

        [Test]
        public void BookExistsShouldReturnFalse()
        {
            const int id = -1;

            Assert.AreEqual(commentService.BookExists(id), false);
        }

        [Test]
        public void AddCommentShouldNotAddCommentIfBookDoesNotExist()
        {
            const int bookId = -1;
            const string userId = "abc";
            const string userName = "";

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            commentService.AddComment(bookId, userName, "");

            Assert.AreEqual(context.Comments.ToList().Count() == 0, true);
        }

        [Test]
        public void AddCommentShouldNotAddCommentIfUserDoesNotExist()
        {
            const string userName = "";

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns((User)null);

            const int id = 1;

            AddTestBook(id);

            commentService.AddComment(id, userName, "");

            Assert.AreEqual(context.Comments.ToList().Count() == 0, true);
        }

        [Test]
        public void AddCommentShouldAddNewComment()
        {
            string userId = "1";
            string userName = "2";

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int bookId = 1;
            AddTestBook(bookId);

            commentService.AddComment(bookId, userName, "");

            Assert.AreEqual(context.Comments.ToList().Count() == 1, true);
        }

        [Test]
        public void GetCommentReturnsComment()
        {
            string userId = "1";
            string userName = "2";

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int bookId = 1;
            AddTestBook(bookId);

            commentService.AddComment(bookId, userName, "");

            List<Comment> comments = context.Comments.ToList();

            Assert.AreEqual(comments.Count() == 1, true);

            var comment = comments[0];
            int commentId = comment.Id;

            Comment c1 = commentService.GetComment(commentId);

            Assert.AreEqual(c1 != null, true);
        }

        [Test]
        public void GetCommentsReturnsAllCommentsUnderABook()
        {
            string userId = "1";
            string userName = "2";

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int bookId = 1;
            AddTestBook(bookId);

            commentService.AddComment(bookId, userName, "");
            commentService.AddComment(bookId, userName, "");
            commentService.AddComment(bookId, userName, "");

            List<Comment> comments = commentService.GetComments(bookId);

            Assert.AreEqual(comments.Count() == 3, true);

            foreach (var comment in comments)
                Assert.AreEqual(comment.BookId, bookId);
        }

        [Test]
        public void DeleteAllBookCommentsShouldRemoveAllComments()
        {
            string userId = "1";
            string userName = "2";

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int bookId = 1;

            AddTestBook(bookId);

            commentService.AddComment(bookId, userName, "");
            commentService.AddComment(bookId, userName, "");
            commentService.AddComment(bookId, userName, "");

            Assert.True(context.Comments.ToList().Count() == 3);

            commentService.DeleteAllBookComments(bookId);

            Assert.True(context.Comments.ToList().Count() == 0);
        }

        [Test]
        public void DeleteCommentByIdShouldReturnNegativeIfCommentNotFound()
        {
            const int id = -1;
            const string userName = "1";
            const bool isAdmin = false;
            Assert.True(commentService.DeleteCommentById(id, userName, isAdmin) == -1);
        }

        [Test]
        public void DeleteCommentByIdShouldReturnNegativeIfUserNotAdminNorAuthor()
        {
            string userId = "1";
            string userName = "2";
            string testUserName = "Ivan";
            bool isAdmin = false;

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int bookId = 1;
            AddTestBook(bookId);

            commentService.AddComment(bookId, userName, "");

            List<Comment> comments = commentService.GetComments(bookId);
            int commentId = comments[0].Id;

            Assert.True(commentService.DeleteCommentById(commentId, testUserName, isAdmin) == -1);
        }

        [Test]
        public void DeleteCommentByIdShouldReturnBookId()
        {
            string userId = "1";
            string userName = "2";
            bool isAdmin = true;

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int bookId = 1;

            AddTestBook(bookId);

            commentService.AddComment(bookId, userName, "");

            List<Comment> comments = context.Comments.ToList();

            Assert.True(comments.Count() == 1);
            Assert.True(commentService.DeleteCommentById(comments[0].Id, userName, isAdmin) == bookId);
            Assert.True(context.Comments.ToList().Count() == 0);
        }

        [Test]
        public void GetCommentsExtendedInfoShouldReturnCommentsWithAdditionalInfo()
        {
            string userId = "1";
            string userName = "2";

            User user = AddTestUser(userId, userName);

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            userServiceMock.Setup(x => x.GetUserRoleColor(userId))
                           .Returns("black");

            const int bookId = 1;

            AddTestBook(bookId);

            commentService.AddComment(bookId, userName, "");
            commentService.AddComment(bookId, userName, "");
            commentService.AddComment(bookId, userName, "");

            Assert.True(context.Comments.ToList().Count() == 3);

            List<CommentExtendedViewModel> commentsExtra = commentService.GetCommentsExtendedInfo(bookId);

            Assert.True(commentsExtra.Count() == 3);

            foreach (var comment in commentsExtra)
                Assert.AreEqual(comment.Data.BookId, bookId);
        }

        public Book AddTestBook(int id)
        {
            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            return book;
        }

        public User AddTestUser(string userId, string userName)
        {
            User user = new User()
            {
                Id = userId,
                UserName = userName,
                Gender = "idk",
                BirthDate = DateTime.Now,
                NormalizedUserName = "NAME",
                Email = "dedede",
                NormalizedEmail = "EMAIL",
                EmailConfirmed = false,
                PasswordHash = "pishmiqjkata",
                SecurityStamp = "duner",
                ConcurrencyStamp = "pizza",
                PhoneNumber = "ddz",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                LockoutEnabled = false,
                AccessFailedCount = 69
            };

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }
    }
}