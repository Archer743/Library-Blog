using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Data.ViewModels.User;

namespace Library_Web_App.Service.Interfaces
{
	public interface IUserService
	{
        List<UserIndexViewModel> GetAllExtended();

        List<User> GetAll();

        User GetByName(string userName);

        List<BookIndexViewModel> GetLikedBookes(string userName);

        string GetRoleId(string userId);

        Role GetRole(string userId);

        Role GetRoleById(string roleId);

        string GetUserRoleColor(string userId);
    }
}
