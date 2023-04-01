using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library_Web_App.Service
{
	public class LikeService : ILikeService
    {
        private readonly ApplicationContext context;
        private readonly IUserService userService;

        public LikeService(ApplicationContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public bool IsBookLikedByCurUser(int id, string userName)
            => GetBookLikeMadeByUser(id, userName) != null;

        public Like GetBookLikeMadeByUser(int bookId, string userName)
            => context.Likes
                      .Include(c => c.User)
                      .FirstOrDefault(like => like.BookId == bookId && like.User.UserName == userName);

        public List<Like> GetBookLikes(int id)
            => context.Likes
                      .Include(c => c.User)
                      .Where(like => like.BookId == id)
                      .ToList();

        public int GetLikesCount(int id)
            => GetBookLikes(id).Count();

        public void DeleteAllBookLikes(int id)
        {
            List<Like> likes = GetBookLikes(id);

            foreach (Like like in likes)
                context.Likes.Remove(like);

            context.SaveChanges();
        }

        public void Like(int bookId, string userName)
        {
            if (!BookExists(bookId))
                return;

            User user = userService.GetByName(userName);

            if (user == null)
                return;

            context.Likes.Add(new Like(bookId, user.Id));
            context.SaveChanges();
        }

        public void Dislike(int bookId, string userName)
        {
            Like like = GetBookLikeMadeByUser(bookId, userName);

            if (like == null)
                return;

            context.Likes.Remove(like);
            context.SaveChanges();
        }

        public bool BookExists(int bookId)
            => context.Books
                      .FirstOrDefault(book => bookId == book.Id) != null;
    }
}
