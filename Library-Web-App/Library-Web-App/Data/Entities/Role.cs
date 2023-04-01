using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library_Web_App.Data.Entities
{
    public class Role : IdentityRole
    {
        [StringLength(255)]
        public string Color { get; set; }
    }
}
