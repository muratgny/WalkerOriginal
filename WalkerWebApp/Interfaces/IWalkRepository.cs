using WalkerWebApp.Data.Enum;
using WalkerWebApp.Models;

namespace WalkerWebApp.Interfaces
{
    public interface IWalkRepository
    {
        Task<int> GetCountAsync();

        Task<int> GetCountByCategoryAsync(WalkCategory category);

        Task<Walk?> GetByIdAsync(int id);

        Task<Walk?> GetByIdAsyncNoTracking(int id);

        Task<IEnumerable<Walk>> GetAll();

        Task<IEnumerable<Walk>> GetAllWalksByCity(string city);

        Task<IEnumerable<Walk>> GetSliceAsync(int offset, int size);

        Task<IEnumerable<Walk>> GetWalksByCategoryAndSliceAsync(WalkCategory category, int offset, int size);

        bool Add(Walk walk);

        bool Update(Walk walk);

        bool Delete(Walk walk);

        bool Save();
    }
}