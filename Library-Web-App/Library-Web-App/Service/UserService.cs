using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.User;
using Library_Web_App.Service.Interfaces;

namespace Library_Web_App.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext context;

        public UserService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<User> GetAll()
            => context.Users
                      .ToList();

        public List<UserIndexViewModel> GetAllExtended()
            => GetAll().Select(user => new UserIndexViewModel(user.Id, user.UserName, GetRole(user.Id)))
                            .ToList();

        public User GetByName(string userName)
            => context.Users
                      .FirstOrDefault(user => user.UserName == userName);

        public List<BookIndexViewModel> GetLikedBookes(string userName)
            => context.Likes
                      .Where(like => like.User.UserName == userName)
                      .Select(like => new BookIndexViewModel(like.Book))
                      .ToList();

        public string GetRoleId(string userId)
            => context.UserRoles
                      .FirstOrDefault(user_role => user_role.UserId == userId)
                      ?.RoleId ?? null;

        public Role GetRole(string userId)
        {
            string roleId = GetRoleId(userId);

            if (roleId == null)
                return null;

            return GetRoleById(roleId);
        }

        public Role GetRoleById(string roleId)
            => context.Roles
                      .FirstOrDefault(role => role.Id == roleId);

        public string GetUserRoleColor(string userId)
            => GetRole(userId)?.Color ?? "black";
    }
}
