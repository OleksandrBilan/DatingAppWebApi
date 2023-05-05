using DatingApp.DB;
using DatingApp.DB.Models.UserRelated;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;

        public AdminService(UserManager<User> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<VipRequest>> GetVipRequestsAsync()
        {
            return await _dbContext.VipRequests.OrderBy(r => r.CreateDateTime).ToListAsync();
        }

        public async Task ApproveVipRequestAsync(int requestId)
        {
            var request = await _dbContext.VipRequests.Include(r => r.User)
                                                      .Include(r => r.SubscriptionType)
                                                      .FirstOrDefaultAsync(r => r.Id == requestId);
            if (request is null)
                throw new ArgumentException("No request with such id", nameof(requestId));

            var result = await _userManager.AddToRoleAsync(request.User, "VIP");
            if (result.Succeeded)
            {
                var userSubscription = new UserSubscription
                {
                    UserId = request.UserId,
                    SubscriptionTypeId = request.SubscriptionTypeId,
                    CreateDateTime = DateTime.Now,
                    ExpireDateTime = DateTime.Now.AddMonths(request.SubscriptionType.Months),
                };
                _dbContext.UsersSubscriptions.Add(userSubscription);

                _dbContext.VipRequests.Remove(request);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeclineVipRequestAsync(int requestId)
        {
            var request = await _dbContext.VipRequests.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == requestId);
            if (request is null)
                return;

            _dbContext.VipRequests.Remove(request);
            await _dbContext.SaveChangesAsync();
        }
    }
}
