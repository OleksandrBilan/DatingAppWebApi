using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.User
{
    public class UpdateUserInfoDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
