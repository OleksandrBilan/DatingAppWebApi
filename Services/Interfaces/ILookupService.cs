using DatingApp.DB.Models.UserRelated;

namespace DatingApp.Services.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<Sex>> GetSexAsync();
    }
}
