using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Auth
{
    public class CredentialsDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
