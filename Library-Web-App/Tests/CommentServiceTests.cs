using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.Comment;
using Library_Web_App.Service;
using Library_Web_App.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

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

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            Assert.AreEqual(true, commentService.BookExists(id));
        }

        [Test]
        public void BookExistsShouldReturnFalse()
        {
            const int id = -9;

            Assert.AreEqual(false, commentService.BookExists(id));
        }

        [Test]
        public void AddCommentShouldNotAddCommentIfBookDoesNotExist()
        {
            commentService.AddComment(1, "", "");

            Assert.True(context.Comments.ToList().Count() == 0);
        }

        [Test]
        public void AddCommentShouldNotAddCommentIfUserDoesNotExist()
        {
            string userName = "";

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns((User)null);

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            commentService.AddComment(id, userName, "");

            Assert.True(context.Comments.ToList().Count() == 0);
        }

        [Test]
        public void AddCommentShouldAddNewComment()
        {
            string userName = "2";

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(new User());

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            commentService.AddComment(id, userName, "");

            Assert.True(context.Comments.ToList().Count() == 1);
        }

        [Test]
        public void GetCommentReturnsComment()
        {
            string userId = "1";
            string userName = "2";

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

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            context.Users.Add(user);
            context.SaveChanges();

            commentService.AddComment(id, userName, "");

            List<Comment> comments = context.Comments.ToList();

            Assert.True(comments.Count() == 1);
            
            var comment = comments[0];
            int commentId = comment.Id;
            
            Assert.True(commentService.GetComment(commentId) != null);
        }

        [Test]
        public void GetCommentsReturnsAllCommentsUnderABook()
        {
            string userId = "1";
            string userName = "2";

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

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            context.Users.Add(user);
            context.SaveChanges();

            commentService.AddComment(id, userName, "");
            commentService.AddComment(id, userName, "");
            commentService.AddComment(id, userName, "");

            List<Comment> comments = commentService.GetComments(id);

            Assert.True(comments.Count() == 3);

            foreach(var comment in comments)
                Assert.AreEqual(comment.BookId, id);
        }

        [Test]
        public void DeleteAllBookCommentsShouldRemoveAllComments()
        {
            string userId = "1";
            string userName = "2";

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

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            context.Users.Add(user);
            context.SaveChanges();

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            commentService.AddComment(id, userName, "");
            commentService.AddComment(id, userName, "");
            commentService.AddComment(id, userName, "");

            Assert.True(context.Comments.ToList().Count() == 3);

            commentService.DeleteAllBookComments(id);

            Assert.True(context.Comments.ToList().Count() == 0);
        }

        [Test]
        public void DeleteCommentByIdShouldReturnNegativeIfCommentNotFound()
        {
            Assert.True(commentService.DeleteCommentById(69) == -1);
        }

        [Test]
        public void DeleteCommentByIdShouldReturnBookId()
        {
            string userId = "1";
            string userName = "2";

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

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            context.Users.Add(user);
            context.SaveChanges();

            commentService.AddComment(id, userName, "");

            List<Comment> comments = context.Comments.ToList();

            Assert.True(comments.Count() == 1);
            Assert.True(commentService.DeleteCommentById(comments[0].Id) == id);
            Assert.True(context.Comments.ToList().Count() == 0);
        }

        [Test]
        public void GetCommentsExtendedInfoShouldReturnCommentsWithAdditionalInfo()
        {
            string userId = "1";
            string userName = "2";

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

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(user);

            userServiceMock.Setup(x => x.GetUserRoleColor(userId))
                           .Returns("black");

            context.Users.Add(user);
            context.SaveChanges();

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            commentService.AddComment(id, userName, "");
            commentService.AddComment(id, userName, "");
            commentService.AddComment(id, userName, "");

            Assert.True(context.Comments.ToList().Count() == 3);

            List<CommentInfoViewModel> commentsExtra = commentService.GetCommentsExtendedInfo(id);

            Assert.True(commentsExtra.Count() == 3);

            foreach (var comment in commentsExtra)
                Assert.AreEqual(comment.Data.BookId, id);
        }
    }
}
