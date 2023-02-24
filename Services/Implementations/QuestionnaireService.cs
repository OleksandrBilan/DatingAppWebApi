using DatingApp.DB;
using DatingApp.DB.Models.Questionnaire;
using DatingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services.Implementations
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly AppDbContext _dbContext;

        private async Task AddAnswersRangeAsync(int questionId, IEnumerable<string> answers)
        {
            var newAnswers = answers.Select(a => new Answer { Name = a, QuestionId = questionId }).ToArray();
            await _dbContext.Answers.AddRangeAsync(newAnswers);
        }

        private async Task RemoveQuestionAnswersAsync(int questionId)
        {
            var answers = await _dbContext.Answers.Where(a => a.QuestionId == questionId).ToArrayAsync();
            _dbContext.Answers.RemoveRange(answers);
        }

        public QuestionnaireService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Question> AddQuestionAsync(string question, IEnumerable<string> answers)
        {
            if (string.IsNullOrWhiteSpace(question)) 
                throw new ArgumentNullException(nameof(question));

            foreach (var answer in answers)
            {
                if (string.IsNullOrWhiteSpace(answer))
                    throw new ArgumentNullException(nameof(answers));
            }

            var existingQuestion = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Name == question);
            if (existingQuestion is null)
            {
                var newQuestion = new Question { Name = question };
                _dbContext.Questions.Add(newQuestion);
                await _dbContext.SaveChangesAsync();

                await AddAnswersRangeAsync(newQuestion.Id, answers);
                await _dbContext.SaveChangesAsync();

                return newQuestion;
            }
            return existingQuestion;
        }

        public async Task DeleteQuestionAsync(int questionId)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
            if (question is not null)
            {
                await RemoveQuestionAnswersAsync(questionId);
                _dbContext.Questions.Remove(question);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ChangeQuestionAsync(int questionId, string newQuestion, IEnumerable<string> newAnswers)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
            if (question is null)
                throw new ArgumentException($"Question with id {questionId} does not exist");

            question.Name = newQuestion;
            await RemoveQuestionAnswersAsync(questionId);
            await AddAnswersRangeAsync(questionId, newAnswers);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionnaireAsync()
        {
            var questions = await _dbContext.Questions.Include(q => q.Answers).ToListAsync();
            return questions;
        }
    }
}
