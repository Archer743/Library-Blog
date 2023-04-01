using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.User;
using Library_Web_App.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Library_Web_App.Service
{
    public class UserService
    {
        private readonly ApplicationContext context;

        public UserService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<UserIndexViewModel> GetAll()
            => GetAllUsers().Select(user => new UserIndexViewModel(user.Id, user.UserName, GetUserRole(user.Id)))
                            .ToList();

        public List<User> GetAllUsers()
            => context.Users
                      .ToList();

        public User GetById(string id)
            => context.Users
                      .FirstOrDefault(user => user.Id == id);

        public List<Book> GetLikedBookes(string id)
            => context.Likes
                      .Where(like => like.UserId == id)
                      .Select(like => like.Book)
                      .ToList();

        public Role GetUserRole(string userId)
        {
            string roleId = context.UserRoles
                                   .FirstOrDefault(user_role => user_role.UserId == userId)
                                   .RoleId;

            return context.Roles
                          .FirstOrDefault(role => role.Id == roleId);
        }
    }
}
