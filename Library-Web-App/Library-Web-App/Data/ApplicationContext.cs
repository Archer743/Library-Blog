using Library_Web_App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace Library_Web_App.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
