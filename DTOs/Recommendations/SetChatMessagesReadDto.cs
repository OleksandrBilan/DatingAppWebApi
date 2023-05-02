using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Recommendations
{
    public class SetChatMessagesReadDto
    {
        [Required]
        public int ChatId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
