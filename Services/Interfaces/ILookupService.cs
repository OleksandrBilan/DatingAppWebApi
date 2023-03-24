using DatingApp.DB.Models.Locations;
using DatingApp.DB.Models.UserRelated;

namespace DatingApp.Services.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<Sex>> GetSexAsync();

        Task<IEnumerable<Country>> GetCountriesAsync();

        Task<IEnumerable<City>> GetCitiesAsync();
    }
}
