using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager) {
            _userManager = userManager;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);   
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _userManager.FindByNameAsync(username).ConfigureAwait(false);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
           return await _userManager.UpdateAsync(user).ConfigureAwait(false);
        }
        public async Task<List<Guid>> GetAllUserIdsAsync()
        {
            return await _userManager.Users
                .Select(u => u.Id)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
