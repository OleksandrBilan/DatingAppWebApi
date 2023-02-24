using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Questionnaire
{
    public class ChangeQuestionDto
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public IEnumerable<string> Answers { get; set; }
    }
}
