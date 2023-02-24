namespace DatingApp.DB.Models.Questionnaire
{
    public class Question
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
