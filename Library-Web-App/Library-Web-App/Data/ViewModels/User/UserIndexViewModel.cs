using Library_Web_App.Data.Entities;

namespace Library_Web_App.Data.ViewModels.User
{
    public class UserIndexViewModel
    {
        public UserIndexViewModel(string id, string userName, Role role)
        {
            Id = id;
            UserName = userName;
            Role = role;
        }

        public string Id { get; private set; }

        public string UserName { get; private set; }

        public Role Role { get; set; }
    }
}
