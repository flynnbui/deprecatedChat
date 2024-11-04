using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Models;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class KeyRepository : IKeyRepository
    {
        private readonly ApplicationDbContext _context;

        public KeyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task StoreKeyBundleAsync(KeyBundle keyBundle)
        {
            // Check if key bundle exists
            var existingBundle = await _context.KeyBundles.FindAsync(keyBundle.UserId);
            if (existingBundle != null)
            {
                _context.KeyBundles.Remove(existingBundle);
            }

            _context.KeyBundles.Add(keyBundle);
            await _context.SaveChangesAsync();
        }

        public Task<KeyBundle> GetKeyBundleAsync(Guid userId)
        {
            return _context.KeyBundles
                .Include(k => k.PreKey)
                .Include(k => k.SignedPreKey)
                .FirstOrDefaultAsync(k => k.UserId == userId);
        }
    }
}
