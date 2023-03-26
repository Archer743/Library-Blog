using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library_Web_App.Data.Entities
{
    public class User : IdentityUser
    {
        [StringLength(6)]
        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
