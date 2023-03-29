using System.ComponentModel.DataAnnotations;

namespace Library_Web_App.Data.Entities
{
    public class Book
    {
        public Book() { }

        public Book(int id, string title, string preview, string link, string genre,
                    string author, int pages, int yearOfPublication)
        {
            Id = id;
            Title = title;
            Preview = preview;
            Link = link;
            Genre = genre;
            Author = author;
            Pages = pages;
            YearOfPublication = yearOfPublication;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string Preview { get; set; }

        public string Link { get; set; }

        [StringLength(100)]
        public string Genre { get; set; }

        [StringLength(150)]
        public string Author { get; set; }

        public int Pages { get; set; }

        public int YearOfPublication { get; set; }
    }
}
