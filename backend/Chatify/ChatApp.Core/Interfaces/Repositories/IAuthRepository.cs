using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<IdentityResult> CreateUserAsync(string username, string password);
        Task<(SignInResult Result, string ErrorMessage)> LoginAsync(string username, string password);
    }
}
