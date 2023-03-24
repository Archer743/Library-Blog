using Library_Web_App.Data;
using Library_Web_App.Data.Entities;

namespace Library_Web_App.Service
{
    public class BookService
    {
        private ApplicationContext context;

        public BookService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<Book> GetAll()
            => context.Books.ToList();

        public List<Book> GetAllByGenre(string genre)
        {
            return context.Books
                            .Where(book => book.Genre == genre)
                            .ToList();
        }

        public List<Book> GetAllByAuthor(string author)
        {
            return context.Books
                            .Where(book => book.Author == author)
                            .ToList();
        }

        public List<Book> GetAllByYearOfPublication(int yearOfPublication)
        {
            return context.Books
                            .Where(book => book.YearOfPublication == yearOfPublication)
                            .ToList();
        }

        public Book GetById(int id)
            => context.Books.FirstOrDefault(book => id == book.Id);

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

        public void Delete(Book book)
        {
            context.Books.Remove(book);
            context.SaveChanges();
        }
    }
}
