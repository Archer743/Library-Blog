using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Comment;
using Library_Web_App.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Library_Web_App.Service
{
	public class CommentService : ICommentService
    {
        private readonly ApplicationContext context;
        private readonly IUserService userService;

        public CommentService(ApplicationContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public List<Comment> GetComments(int id)
            => context.Comments
                      .Include(c => c.User)
                      .Where(comment => comment.BookId == id)
                      .OrderBy(comment => comment.Posted)
                      .ToList();

        public List<CommentExtendedViewModel> GetCommentsExtendedInfo(int id)
            => GetComments(id).Select(comment => new CommentExtendedViewModel(comment, userService.GetUserRoleColor(comment.UserId)))
                              .ToList();

        public BookCommentsViewModel GetBookCommentsViewModel(int bookId)
            => new BookCommentsViewModel(bookId, GetCommentsExtendedInfo(bookId));

        public Comment GetComment(int id)
           => context.Comments
                     .Include(c => c.User)
                     .FirstOrDefault(comment => comment.Id == id);

        public void AddComment(int bookId, string userName, string message)
        {
            if (!BookExists(bookId))
                return;

            User user = userService.GetByName(userName);

            if (user == null)
                return;

            context.Comments.Add(new Comment(bookId, user.Id, message, DateTime.Now));
            context.SaveChanges();
        }

        public void DeleteAllBookComments(int id)
        {
            List<Comment> comments = GetComments(id);

            foreach (Comment comment in comments)
                context.Comments.Remove(comment);

            context.SaveChanges();
        }

        public int DeleteCommentById(int id, string userName, bool isAdmin)
        {
            var comment = GetComment(id);

            if (comment == null || (!isAdmin && comment.User.UserName != userName))
                return -1;

            int bookId = comment.BookId;

            context.Comments.Remove(comment);
            context.SaveChanges();

            return bookId;
        }

        public bool BookExists(int bookId)
            => context.Books
                      .FirstOrDefault(book => bookId == book.Id) != null;
    }
}
