﻿using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Web.Models
{
    public class SignInInput
    {
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
