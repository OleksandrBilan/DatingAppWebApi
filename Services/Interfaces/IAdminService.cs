using DatingApp.DB.Models.UserRelated;

namespace DatingApp.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<VipRequest>> GetVipRequestsAsync();

        Task ApproveVipRequestAsync(int requestId);

        Task DeclineVipRequestAsync(int requestId);
    }
}
