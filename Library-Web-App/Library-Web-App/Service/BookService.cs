using Library_Web_App.Data;
using Library_Web_App.Data.Entities;
using Library_Web_App.Data.ViewModels.Book;
using Library_Web_App.Migrations;

namespace Library_Web_App.Service
{
    public class BookService
    {
        private readonly ApplicationContext context;

        public BookService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<IndexViewModel> GetAll()
            => context.Books.Select(book => new IndexViewModel(book)).ToList();

        public List<Book> GetAllByGenre(string genre)
            => context.Books
                      .Where(book => book.Genre == genre)
                      .ToList();

        public List<Book> GetAllByAuthor(string author)
            => context.Books
                      .Where(book => book.Author == author)
                      .ToList();

        public List<Book> GetAllByYearOfPublication(int yearOfPublication)
            => context.Books
                      .Where(book => book.YearOfPublication == yearOfPublication)
                      .ToList();

        public Book GetById(int id)
            => context.Books
                      .FirstOrDefault(book => id == book.Id);

        public InfoViewModel GetByIdExtendedInfo(string userName, int id)
            => new InfoViewModel(GetById(id), GetLikesCount(id), GetComments(id), IsBookLikedByCurUser(id, userName));

        public bool IsBookLikedByCurUser(int id, string userName)
            => GetBookLikeMadeByUser(id, userName) != null;

        public Like GetBookLikeMadeByUser(int bookId, string userName)
            => context.Likes
                      .FirstOrDefault(like => like.BookId == bookId && like.User.UserName == userName);

        public List<Like> GetBookLikes(int id)
            => context.Likes
                      .Where(like => like.BookId == id)
                      .ToList();

        public int GetLikesCount(int id)
            => GetBookLikes(id).Count();

        public List<Comment> GetComments(int id)
            => context.Comments
                      .Where(comment => comment.BookId == id)
                      .ToList();

        public Comment GetComment(int id)
            => context.Comments
                      .FirstOrDefault(comment => comment.Id == id);

        public void AddComment(int bookId, string userName, string message)
        {
            context.Comments.Add(new Comment(bookId, GetUser(userName).Id, message, DateTime.Now));
            context.SaveChanges();
        }

        public void DeleteAllBookLikes(int id)
        {
            List<Like> likes = GetBookLikes(id);
            
            foreach(Like like in likes)
                context.Likes.Remove(like);

            context.SaveChanges();
        }

        public void DeleteAllBookComments(int id)
        {
            List<Comment> comments = GetComments(id);

            foreach (Comment comment in comments)
                context.Comments.Remove(comment);

            context.SaveChanges();
        }

        public int DeleteCommentById(int id)
        {
            var comment = GetComment(id);
            int bookId = comment.BookId;

            context.Comments.Remove(comment);
            context.SaveChanges();

            return bookId;
        }

        public User GetUser(string userName)
            => context.Users
                      .FirstOrDefault(user => user.UserName == userName);

        public void Add(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
        }

        public void Edit(Book book)
        {
            context.Books.Update(book);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Book book = GetById(id);

            context.Books.Remove(book);
            DeleteAllBookLikes(id);
            DeleteAllBookComments(id);
            
            context.SaveChanges();
        }

        public void Like(int bookId, string userName)
        {
            context.Likes.Add(new Like(bookId, GetUser(userName).Id));
            context.SaveChanges();
        }

        public void Dislike(int bookId, string userName)
        {
            context.Likes.Remove(GetBookLikeMadeByUser(bookId, userName));
            context.SaveChanges();
        }
    }
}
