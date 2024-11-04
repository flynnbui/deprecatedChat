using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserAsync(string username);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<List<Guid>> GetAllUserIdsAsync();
    }
}
