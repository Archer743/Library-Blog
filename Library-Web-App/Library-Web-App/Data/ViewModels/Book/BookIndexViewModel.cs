using BookClass = Library_Web_App.Data.Entities.Book;

namespace Library_Web_App.Data.ViewModels.Book
{
	public class BookIndexViewModel
	{
		public BookIndexViewModel(BookClass book)
		{
			Id = book.Id;
			Title = book.Title;
			Author = book.Author;
			Genre = book.Genre;
			YearOfPublication = book.YearOfPublication;
			Pages = book.Pages;
		}

		public int Id { get; private set; }

		public string Title { get; private set; }

		public string Author { get; private set; }

		public string Genre { get; private set; }
		
		public int YearOfPublication { get; private set; }
		
		public int Pages { get; private set; }
	}
}
