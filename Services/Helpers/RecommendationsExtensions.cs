using DatingApp.DB;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services.Helpers
{
    public static class RecommendationsExtensions
    {
        public static IQueryable<User> FormBaseQuery(this AppDbContext dbContext, FiltersDto filters)
        {
            if (filters is null || filters.UserId is null) 
                throw new ArgumentNullException(nameof(filters));

            return dbContext.Users.Include(u => u.Country)
                                  .Include(u => u.City)
                                  .SelectMany(
                                      user => dbContext.UserRoles.Where(userRoleMapEntry => user.Id == userRoleMapEntry.UserId).DefaultIfEmpty(),
                                      (user, roleMapEntry) => new { User = user, RoleMapEntry = roleMapEntry })
                                  .SelectMany(
                                      x => dbContext.Roles.Where(role => role.Id == x.RoleMapEntry.RoleId).DefaultIfEmpty(),
                                      (x, role) => new { User = x.User, Role = role })
                                  .Where(x => x.Role.Name == "User" && x.User.EmailConfirmed && x.User.Id != filters.UserId)
                                  .Select(x => x.User);
        }

        public static IEnumerable<RecommendedUser> GetRecommendedUsersByFiltersAsync(this IQueryable<User> query, FiltersDto filters)
        {
            if (filters is null)
                return Array.Empty<RecommendedUser>();

            if (filters.MinAge is not null)
            {
                query = query.Where(u => u.BirthDate.Year <= DateTime.Today.Year - filters.MinAge);
            }

            if (filters.MaxAge is not null)
            {
                query = query.Where(u => u.BirthDate.Year >= DateTime.Today.Year - filters.MaxAge);
            }

            if (!string.IsNullOrEmpty(filters.CountryCode))
            {
                query = query.Where(u => u.CountryCode == filters.CountryCode);
            }

            if (filters.CityId is not null)
            {
                query = query.Where(u => u.CityId == filters.CityId);
            }

            if (filters.PreferedSexId is not null && filters.PreferedSexId != 1)
            {
                query = query.Where(u => u.SexId == filters.PreferedSexId);
            }

            return query.Select(u => new RecommendedUser { User = u, SimilarityScore = 0d }).ToList();
        }

        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) 
                age--;
            return age;
        }

        public static IEnumerable<RecommendedUser> CalculateUsersSimilarityScoreAsync(this IEnumerable<RecommendedUser> recommendedUsers, FiltersDto filters, AppDbContext dbContext)
        {
            if (filters is null || filters.UserId is null)
                return recommendedUsers;

            var currentUser = dbContext.Users.FirstOrDefault(u => u.Id == filters.UserId);
            if (currentUser is null)
                return recommendedUsers;

            int currentUserAge = CalculateAge(currentUser.BirthDate);
            foreach (var recommendedUser in recommendedUsers)
            {
                var user = recommendedUser.User;

                int userAge = CalculateAge(user.BirthDate);
                int maxAge = userAge > currentUserAge ? userAge : currentUserAge;
                double ageSimilarityScore = 1 - Math.Abs(currentUserAge - userAge) / (double)maxAge;

                int countriesSimilarityScore = currentUser.CountryCode == user.CountryCode ? 1 : 0;
                int citiesSimilarityScore = currentUser.CityId == user.CityId ? 1 : 0;

                recommendedUser.SimilarityScore = ageSimilarityScore * 0.5d + countriesSimilarityScore * 0.3d + citiesSimilarityScore * 0.2d;
            }

            return recommendedUsers;
        }

        public static IEnumerable<RecommendedUser> CalculateUsersSimilarityByQuestionnaireScoreAsync(this IEnumerable<RecommendedUser> recommendedUsers, FiltersDto filters, AppDbContext dbContext)
        {
            if (filters is null || !filters.UseQuestionnaire.HasValue || !filters.UseQuestionnaire.Value || filters.UserId is null)
                return recommendedUsers;

            int questionsCount = dbContext.Questions.Count();
            var currentUserAnswers = dbContext.UsersQuestionsAnswers.Where(x => x.UserId == filters.UserId).ToList();
            if (!currentUserAnswers.Any())
                return recommendedUsers;

            foreach (var recommendedUser in recommendedUsers)
            {
                var userAnswers = dbContext.UsersQuestionsAnswers.Where(x => x.UserId == recommendedUser.User.Id).ToList();
                if (userAnswers.Any())
                {
                    int intersectionsCount = 0;
                    foreach (var currentUserAnswer in currentUserAnswers)
                    {
                        if (userAnswers.Any(x => x.QuestionId == currentUserAnswer.QuestionId && x.AnswerId == currentUserAnswer.AnswerId))
                            intersectionsCount++;
                    }
                    double similarity = (double)intersectionsCount / questionsCount;
                    recommendedUser.SimilarityScore = recommendedUser.SimilarityScore == 0 ? similarity : (recommendedUser.SimilarityScore + similarity) / 2;
                }
            }

            return recommendedUsers;
        }

        public static IEnumerable<RecommendedUser> OrderBySimilarityDescending(this IEnumerable<RecommendedUser> recommendedUsers)
        {
            return recommendedUsers.OrderByDescending(x => x.SimilarityScore);
        }
    }
}
