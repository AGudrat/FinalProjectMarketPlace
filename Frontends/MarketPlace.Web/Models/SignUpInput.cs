
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Web.Models
{
    public class SignUpInput
    {
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
        [Display(Name = "Remember me")]
        public bool IsRemember { get; set; }
    }
}
