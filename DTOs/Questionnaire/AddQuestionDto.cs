using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Questionnaire
{
    public class AddQuestionDto
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public IEnumerable<string> Answers { get; set; }
    }
}
