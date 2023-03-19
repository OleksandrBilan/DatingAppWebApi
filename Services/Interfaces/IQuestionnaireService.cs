using DatingApp.DB.Models.Questionnaire;
using DatingApp.DTOs.Questionnaire;

namespace DatingApp.Services.Interfaces
{
    public interface IQuestionnaireService
    {
        Task<Question> AddQuestionAsync(string question, IEnumerable<string> answers);

        Task<IEnumerable<Question>> GetQuestionnaireAsync();

        Task DeleteQuestionAsync(int questionId);

        Task ChangeQuestionAsync(int questionId, string newQuestion);

        Task AddAnswerAsync(int questionId, string newAnswer);

        Task DeleteAnswerAsync(int answerId);

        Task ChangeAnswerAsync(int answerId, string newAnswer);

        Task<IEnumerable<UserQuestionAnswer>> GetUserAnswersAsync(string userId);

        Task UpdateUserAnswersAsync(string userId, IEnumerable<QuestionAnswerDto> answers);
    }
}
