using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Questionnaire
{
    public class QuestionAnswerDto
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int AnswerId { get; set; }
    }
}
