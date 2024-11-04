using ChatApp.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Core.Interfaces.Services
{
    public interface ITokenClaimService
    {
        Task<(string Token, string RawFingerprint)> GenerateJwtToken(string username);
        string GenerateRefreshToken();
        Task<TokenValidationResult>? GetTokenPrincipal(string token);
    }
}
