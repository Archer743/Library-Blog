using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Service;
using Library_Web_App.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests
{
    public class LikeServiceTests
    {
        private ApplicationContext context;
        private Mock<IUserService> userServiceMock = new Mock<IUserService>();
        private LikeService likeService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                          .UseInMemoryDatabase("TestDB").Options;

            context = new ApplicationContext(options);

            likeService = new LikeService(context, userServiceMock.Object);
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

            Assert.AreEqual(true, likeService.BookExists(id));
        }

        [Test]
        public void BookExistsShouldReturnFalse()
        {
            const int id = -69;

            Assert.AreEqual(false, likeService.BookExists(id));
        }

        [Test]
        public void LikeShouldNotAddLikeIfBookDoesNotExist()
        {
            likeService.Like(1, "");

            Assert.True(context.Likes.ToList().Count() == 0);
        }

        [Test]
        public void LikeShouldNotAddLikeIfUserDoesNotExist()
        {
            string userName = "";
            
            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns((User)null);

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();
                
            likeService.Like(id, userName);

            Assert.True(context.Likes.ToList().Count() == 0);
        }

        [Test]
        public void LikeShouldAddNewLike()
        {
            string userName = "2";

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(new User());

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            likeService.Like(id, userName);

            Assert.True(context.Likes.ToList().Count() == 1);
        }

        [Test]
        public void DislikeShouldNotRemoveLikeIfLikeNotFound()
        {
            string userName = "2";

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(new User());

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            likeService.Like(id, userName);
            likeService.Dislike(-9, userName);

            Assert.True(context.Likes.ToList().Count() == 1);
        }

        [Test]
        public void DislikeShouldRemoveLike()
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

            likeService.Like(id, userName);

            likeService.Dislike(id, userName);

            Assert.True(context.Likes.ToList().Count() == 0);
        }

        [Test]
        public void IsBookLikedByCurUserReturnsTrueIfHeHasLikedTheBook()
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

            likeService.Like(id, userName);

            Assert.AreEqual(true, likeService.IsBookLikedByCurUser(id, userName));
        }

        [Test]
        public void IsBookLikedByCurUserReturnsFalseIfHeHasNotLikedTheBook()
        {
            string userName = "2";

            userServiceMock.Setup(x => x.GetByName(userName))
                           .Returns(new User());

            const int id = 1;

            Book book = new Book(id, "title", "preview", "link",
                                 "genre", "author", 10, 2000);

            context.Books.Add(book);
            context.SaveChanges();

            likeService.Like(id, userName);

            Assert.True(context.Likes.ToList().Count() == 1);
            Assert.AreEqual(false, likeService.IsBookLikedByCurUser(id, userName));
        }

        [Test]
        public void GetBookLikeMadeByUserReturnsUser()
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

            likeService.Like(id, userName);

            Assert.True(likeService.GetBookLikeMadeByUser(id, userName) != null);
        }

        [Test]
        public void DeleteAllBookLikesShouldRemoveAllLikes()
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

            likeService.Like(id, userName);
            likeService.Like(id, userName);
            likeService.Like(id, userName);

            Assert.True(context.Likes.ToList().Count() == 3);
            
            likeService.DeleteAllBookLikes(id);
            
            Assert.True(context.Likes.ToList().Count() == 0);
        }

        [Test]
        public void GetBookLikesReturnsLikes()
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

            likeService.Like(id, userName);
            likeService.Like(id, userName);
            likeService.Like(id, userName);

            List<Like> likes = likeService.GetBookLikes(id);

            Assert.True(likes.Count() == 3);

            foreach (Like like in likes)
                Assert.AreEqual(id, like.BookId);
        }

        [Test]
        public void GetLikesCountReturnsRightCount()
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

            likeService.Like(id, userName);
            likeService.Like(id, userName);
            likeService.Like(id, userName);

            Assert.True(likeService.GetLikesCount(id) == 3);
        }
    }
}