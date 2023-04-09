using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Library_Web_App.Data.Entities
{
    public class Role : IdentityRole
    {
        public Role(string name, string color)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            NormalizedName = name.ToUpper();
            Color = color;
        }

        [StringLength(255)]
        public string Color { get; set; }
    }
}
