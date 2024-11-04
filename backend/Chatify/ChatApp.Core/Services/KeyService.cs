using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Services;
using ChatApp.Core.Models;

namespace ChatApp.Core.Services
{
    public class KeyService : IKeyService
    {
        private readonly IKeyRepository _keyRepository;

        public KeyService(IKeyRepository keyRepository)
        {
            _keyRepository = keyRepository;
        }

        public Task StoreKeyBundleAsync(KeyBundle keyBundle)
        {
            return _keyRepository.StoreKeyBundleAsync(keyBundle);
        }

        public Task<KeyBundle> GetKeyBundleAsync(Guid userId)
        {
            return _keyRepository.GetKeyBundleAsync(userId);
        }
    }
}
