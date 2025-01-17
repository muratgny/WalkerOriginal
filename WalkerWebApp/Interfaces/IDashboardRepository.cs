using WalkerWebApp.Models;

namespace WalkerWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Walk>> GetAllUserWalks();
        Task<List<Club>> GetAllUserClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
