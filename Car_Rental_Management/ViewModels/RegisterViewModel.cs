
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Management.ViewModels
{
    public class RegisterViewModel
    {
        [Required, Display(Name = "Username")]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}

