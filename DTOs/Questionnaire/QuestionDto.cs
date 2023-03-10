namespace DatingApp.DTOs.Questionnaire
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<IdValueDto> Answers { get; set; }
    }
}
