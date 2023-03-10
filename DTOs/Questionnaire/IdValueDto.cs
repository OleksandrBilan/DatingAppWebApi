using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Questionnaire
{
    public class IdValueDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
