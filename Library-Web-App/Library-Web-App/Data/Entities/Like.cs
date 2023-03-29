using System.ComponentModel.DataAnnotations;

namespace Library_Web_App.Data.Entities
{
    public class Like
    {
        public Like() { }

        public Like(int bookId, string userId)
        {
            BookId = bookId;
            UserId = userId;
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string UserId { get; set; }

        public User User { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
