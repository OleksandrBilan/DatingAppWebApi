using DatingApp.DB;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services.Helpers.Recommendations
{
    public class FiltersRecommendationStrategy : IRecommendationStrategy
    {
        private readonly AppDbContext _dbContext;

        public FiltersRecommendationStrategy(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IQueryable<User> FormBaseQuery()
        {
            return _dbContext.Users.Include(u => u.Country).Include(u => u.City)
                                   .SelectMany(
                                       user => _dbContext.UserRoles.Where(userRoleMapEntry => user.Id == userRoleMapEntry.UserId).DefaultIfEmpty(),
                                       (user, roleMapEntry) => new { User = user, RoleMapEntry = roleMapEntry })
                                   .SelectMany(
                                       x => _dbContext.Roles.Where(role => role.Id == x.RoleMapEntry.RoleId).DefaultIfEmpty(),
                                       (x, role) => new { User = x.User, Role = role })
                                   .Where(x => x.Role.Name == "User" && x.User.EmailConfirmed)
                                   .Select(x => x.User);
        }

        private static IQueryable<User> ApplyFilters(FiltersDto filters, IQueryable<User> query)
        {
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

            return query;
        }

        public async Task<IEnumerable<Tuple<User, double>>> GetRecommendedUsersAsync(FiltersDto filters)
        {
            if (filters is null)
                throw new ArgumentNullException(nameof(filters));

            var query = FormBaseQuery();
            query = ApplyFilters(filters, query);
            return await query.Select(u => new Tuple<User, double>(u, 0.0d)).ToListAsync();
        }
    }
}
