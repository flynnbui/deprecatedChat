using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Services;
using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace ChatApp.Core.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _userRepository.GetUserAsync(id).ConfigureAwait(false);
        }

        public async Task<User> GetUser(string username)
        {
            return await _userRepository.GetUserAsync(username).ConfigureAwait(false);
        }

        public async Task<IdentityResult> UpdateUser(User user)
        {
           return await _userRepository.UpdateUserAsync(user).ConfigureAwait(false);
        }
        public Task<List<Guid>> GetAllUserIdsAsync()
        {
            return _userRepository.GetAllUserIdsAsync();
        }
    }
}
