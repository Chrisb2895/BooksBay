using System.ComponentModel.DataAnnotations;

namespace LibraryManager.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password must match password")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}