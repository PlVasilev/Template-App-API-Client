using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Server.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9-]{3,}$")]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }
    }
}
