using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Migrations;

namespace Library_Web_App.Service
{
    public class UserService
    {
        private readonly ApplicationContext context;

        public UserService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<User> GetAll()
            => context.Users.ToList();

        public User GetById(string id)
            => context.Users
                      .FirstOrDefault(user => user.Id == id);

        public List<Book> GetLikedBookes(string id)
            => context.Likes
                      .Where(like => like.UserId == id)
                      .Select(like => like.Book)
                      .ToList();
    }
}
