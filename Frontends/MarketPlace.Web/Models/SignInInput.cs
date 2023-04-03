using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Web.Models
{
    public class SignInInput
    {
        [Required]
        [Display(Name = "Email adresiniz")]
        public string Email { get; set; } = null!;
        [Required]
        [Display(Name = "Şifreniz")]
        public string Password { get; set; } = null!;
        [Display(Name = "Beni hatırla")]
        public bool IsRemember { get; set; }
    }
}
