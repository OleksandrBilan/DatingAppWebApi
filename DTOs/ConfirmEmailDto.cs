using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class ConfirmEmailDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
