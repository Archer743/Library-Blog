using System.ComponentModel.DataAnnotations;

namespace Library_Web_App.Data.Entities
{
    public class Comment
    {
        public Comment() { }

        public Comment(int bookId, string userId, string message, DateTime posted)
        {
            BookId = bookId;
            UserId = userId;
            Message = message;
            Posted = posted;
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string UserId { get; set; }

        public User User { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public DateTime Posted { get; set; }
    }
}
