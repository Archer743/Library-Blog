using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Comment;
using Library_Web_App.Data;
using System.Net;

namespace Library_Web_App.Service.Interfaces
{
	public interface ICommentService
	{
            List<Comment> GetComments(int id);

            List<CommentInfoViewModel> GetCommentsExtendedInfo(int id);

            Comment GetComment(int id);

            void AddComment(int bookId, string userName, string message);

            void DeleteAllBookComments(int id);

            int DeleteCommentById(int id);

            bool BookExists(int bookId);
    }
}
