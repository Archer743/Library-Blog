using Library_Web_App.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace Library_Web_App.Data
{
    public class ApplicationContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Like> Likes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
