using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Questionnaire
{
    public class UpdateUserAnswersDto
    {
        [Required]
        public string UserId { get; set; }

        public IEnumerable<QuestionAnswerDto> Answers { get; set; }
    }
}
