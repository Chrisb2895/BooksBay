﻿using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksBay.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }

        public string CurrentLoggedUser { get; set; }
    }
}