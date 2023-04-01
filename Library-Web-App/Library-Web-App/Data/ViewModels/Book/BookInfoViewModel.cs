using Library_Web_App.Data.ViewModels.Comment;
using BookClass = Library_Web_App.Data.Entities.Book;

namespace Library_Web_App.Data.ViewModels.Book
{
    public class BookInfoViewModel
    {
        public BookInfoViewModel(BookClass book, int likes, List<CommentInfoViewModel> comments, bool likedByCurUser)
        {
            Book = book;
            Likes = likes;
            Comments = comments;
            LikedByCurUser = likedByCurUser;
        }

        public BookClass Book { get; private set; }

        public int Likes { get; private set;  }

        public bool LikedByCurUser { get; private set; }

        public List<CommentInfoViewModel> Comments { get; private set; }
    }
}
