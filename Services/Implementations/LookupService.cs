using DatingApp.DB;
using DatingApp.DB.Models.Locations;
using DatingApp.DB.Models.UserRelated;
using DatingApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services.Implementations
{
    public class LookupService : ILookupService
    {
        private readonly AppDbContext _dbContext;

        public LookupService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _dbContext.Cities.ToListAsync();
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync()
        {
            return await _dbContext.Countries.ToListAsync();
        }

        public async Task<IEnumerable<Sex>> GetSexAsync()
        {
            return await _dbContext.Sex.ToListAsync();
        }

        public async Task<IEnumerable<SubscriptionType>> GetSubscriptionTypesAsync()
        {
            return await _dbContext.SubscriptionTypes.ToListAsync();
        }
    }
}
