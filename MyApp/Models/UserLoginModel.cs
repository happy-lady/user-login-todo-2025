using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class UserLoginModel
    {
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Username must be at least 6 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }  // Optional, for "Remember me" functionality
    }
}

