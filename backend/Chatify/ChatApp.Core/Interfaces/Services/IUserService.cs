using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<User> GetUser(Guid id);
        Task<User> GetUser(string username);
        Task<IdentityResult> UpdateUser(User user);
        Task<List<Guid>> GetAllUserIdsAsync();

    }
}
