using Library_Web_App.Data.Entities;

namespace Library_Web_App.Service.Interfaces
{
	public interface ILikeService
	{
        bool IsBookLikedByCurUser(int id, string userName);

        Like GetBookLikeMadeByUser(int bookId, string userName);

        List<Like> GetBookLikes(int id);

        int GetLikesCount(int id);

        void DeleteAllBookLikes(int id);

        void Like(int bookId, string userName);

        void Dislike(int bookId, string userName);

        bool BookExists(int bookId);
    }
}
