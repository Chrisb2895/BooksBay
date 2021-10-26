using System.ComponentModel.DataAnnotations;

namespace LibraryManager.ViewModels
{
    public class ExternalRegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }       

        public string ReturnUrl { get; set; }
    }
}