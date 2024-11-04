using ChatApp.Core.Models;


namespace ChatApp.Core.Interfaces.Repositories
{
    public interface IKeyRepository
    {
        Task StoreKeyBundleAsync(KeyBundle keyBundle);
        Task<KeyBundle> GetKeyBundleAsync(Guid userId);
    }
}
