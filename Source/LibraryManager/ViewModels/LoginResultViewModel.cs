using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManager.ViewModels
{
    public class LoginResultViewModel
    {
        public LoginResultViewModel(bool success, string returnUrl)
        {
            Success = success;
            ReturnUrl = returnUrl;
        }

        public bool Success { get; set; }
        public string ReturnUrl { get; set; }

    }
}