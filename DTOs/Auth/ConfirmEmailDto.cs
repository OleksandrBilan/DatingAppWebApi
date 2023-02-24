using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Auth
{
    public class ConfirmEmailDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
