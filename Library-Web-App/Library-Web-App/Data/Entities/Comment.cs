using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public Book Book { get; set; }

        [StringLength(115)]
        public string Message { get; set; }

        public DateTime Posted { get; set; }
    }
}
