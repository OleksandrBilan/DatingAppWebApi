using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
