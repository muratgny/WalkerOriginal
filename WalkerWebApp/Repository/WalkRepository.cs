using Microsoft.EntityFrameworkCore;
using WalkerWebApp.Data;
using WalkerWebApp.Data.Enum;
using WalkerWebApp.Interfaces;
using WalkerWebApp.Models;

namespace WalkerWebApp.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly ApplicationDbContext _context;

        public WalkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Walk walk)
        {
            _context.Add(walk);
            return Save();
        }

        public bool Delete(Walk walk)
        {
            _context.Remove(walk);
            return Save();
        }

        public async Task<IEnumerable<Walk>> GetAll()
        {
            return await _context.Walks.ToListAsync();
        }

        public async Task<IEnumerable<Walk>> GetAllWalksByCity(string city)
        {
            return await _context.Walks.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(int id)
        {
            return await _context.Walks.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Walks.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Walks.CountAsync();
        }

        public async Task<int> GetCountByCategoryAsync(WalkCategory category)
        {
            return await _context.Walks.CountAsync(r => r.WalkCategory == category);
        }

        public async Task<IEnumerable<Walk>> GetSliceAsync(int offset, int size)
        {
            return await _context.Walks.Include(a => a.Address).Skip(offset).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<Walk>> GetWalksByCategoryAndSliceAsync(WalkCategory category, int offset, int size)
        {
            return await _context.Walks
                .Where(r => r.WalkCategory == category)
                .Include(a => a.Address)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Walk walk)
        {
            _context.Update(walk);
            return Save();
        }
    }
}