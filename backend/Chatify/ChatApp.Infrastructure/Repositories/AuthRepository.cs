using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;


namespace ChatApp.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> CreateUserAsync(string username, string password)
        {
            return await _userManager.CreateAsync(new User { UserName = username }, password).ConfigureAwait(false);
        }

        public async Task<(SignInResult Result, string ErrorMessage)> LoginAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false).ConfigureAwait(false);

            if (result.Succeeded)
            {
                return (result, string.Empty);
            }
            // Handle specific failure reasons
            string errorMessage = result.IsLockedOut ? "User account is locked out." :
                                 result.IsNotAllowed ? "User is not allowed to sign in." :
                                 result.RequiresTwoFactor ? "Two-factor authentication is required." :
                                 "Invalid username or password.";

            return (result, errorMessage);
        }
    }
}
