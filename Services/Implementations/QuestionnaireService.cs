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
                throw new ArgumentException("Question is not valid", nameof(question));

            foreach (var answer in answers)
            {
                if (string.IsNullOrWhiteSpace(answer))
                    throw new ArgumentException("One of the answers is not valid", nameof(answers));
            }

            var newQuestion = new Question { Name = question };
            _dbContext.Questions.Add(newQuestion);
            await _dbContext.SaveChangesAsync();

            await AddAnswersRangeAsync(newQuestion.Id, answers);
            await _dbContext.SaveChangesAsync();

            return newQuestion;
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

        public async Task ChangeQuestionAsync(int questionId, string newQuestion)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
            if (question is null)
                throw new ArgumentException($"Question with id {questionId} does not exist");

            if (question.Name != newQuestion)
            {
                question.Name = newQuestion;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Question>> GetQuestionnaireAsync() => await _dbContext.Questions.Include(q => q.Answers).ToListAsync();

        public async Task AddAnswerAsync(int questionId, string newAnswer)
        {
            var question = await _dbContext.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
            if (question is null)
                throw new ArgumentException($"Question with id {questionId} does not exist");

            question.Answers = await _dbContext.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
            var answer = new Answer() { QuestionId = questionId, Name = newAnswer };
            question.Answers.Add(answer);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAnswerAsync(int answerId)
        {
            var answer = await _dbContext.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
            if (answer is not null)
            {
                _dbContext.Answers.Remove(answer);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ChangeAnswerAsync(int answerId, string newAnswer)
        {
            if (string.IsNullOrWhiteSpace(newAnswer))
                throw new ArgumentException("Answer is not valid", nameof(newAnswer));

            var answer = await _dbContext.Answers.FirstOrDefaultAsync(a => a.Id == answerId);
            if (answer is null)
                throw new ArgumentException("No answer with such id", nameof(answerId));

            if (answer.Name != newAnswer)
            {
                answer.Name = newAnswer;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
