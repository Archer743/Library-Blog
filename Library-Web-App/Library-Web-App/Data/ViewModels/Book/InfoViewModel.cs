using BookClass = Library_Web_App.Data.Entities.Book;

namespace Library_Web_App.Data.ViewModels.Book
{
    public class InfoViewModel
    {
        public InfoViewModel(BookClass book, int likes, bool likedByCurUser)
        {
            Book = book;
            Likes = likes;
            LikedByCurUser = likedByCurUser;
        }

        public BookClass Book { get; private set; }

        public int Likes { get; private set;  }

        public bool LikedByCurUser { get; private set; }
    }
}
