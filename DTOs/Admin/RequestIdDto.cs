using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Admin
{
    public class RequestIdDto
    {
        [Required]
        public int RequestId { get; set; }
    }
}
