using DatingApp.DB.Models.Questionnaire;

namespace DatingApp.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        Task<Question> AddQuestionAsync(string question, IEnumerable<string> answers);

        Task<IEnumerable<Question>> GetQuestionnaireAsync();

        Task DeleteQuestionAsync(int questionId);

        Task ChangeQuestionAsync(int questionId, string newQuestion, IEnumerable<string> newAnswers);
    }
}
