using Library_Web_App.Data;
using Library_Web_App.Data.Entities;

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
    }
}
