using DatingApp.DB.Models;

namespace DatingApp.Services.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<Sex>> GetSexAsync();
    }
}
