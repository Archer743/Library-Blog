using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Comment;
using Library_Web_App.Data;
using System.Net;

namespace Library_Web_App.Service.Interfaces
{
	public interface ICommentService
	{
            List<Comment> GetComments(int id);

            List<CommentExtendedViewModel> GetCommentsExtendedInfo(int id);

            BookCommentsViewModel GetBookCommentsViewModel(int bookId);

            Comment GetComment(int id);

            void AddComment(int bookId, string userName, string message);

            void DeleteAllBookComments(int id);

            int DeleteCommentById(int id, string userName, bool isAdmin);

            bool BookExists(int bookId);
    }
}
