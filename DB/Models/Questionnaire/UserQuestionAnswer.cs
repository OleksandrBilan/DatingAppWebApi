using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Questionnaire
{
    public class UserQuestionAnswer
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
    }
}
