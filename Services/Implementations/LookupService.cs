using DatingApp.DB;
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

        public async Task<IEnumerable<Sex>> GetSexAsync()
        {
            return await _dbContext.Sex.ToListAsync();
        }
    }
}
