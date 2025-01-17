using WalkerWebApp.Data.Enum;
using WalkerWebApp.Models;

namespace WalkerWebApp.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAll();

        Task<IEnumerable<Club>> GetSliceAsync(int offset, int size);

        Task<IEnumerable<Club>> GetClubsByCountry(string country);

        Task<IEnumerable<Club>> GetClubsByCategoryAndSliceAsync(ClubCategory category, int offset, int size);

        Task<List<Country>> GetAllCountries();

        Task<List<City>> GetAllCitiesByCountry(string country);

        Task<Club?> GetByIdAsync(int id);

        Task<Club?> GetByIdAsyncNoTracking(int id);

        Task<IEnumerable<Club>> GetClubByCity(string city);

        Task<int> GetCountAsync();

        Task<int> GetCountByCategoryAsync(ClubCategory category);

        bool Add(Club club);

        bool Update(Club club);

        bool Delete(Club club);

        bool Save();
    }
}