using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class ConfirmEmailDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string ConfirmationToken { get; set; }
    }
}
