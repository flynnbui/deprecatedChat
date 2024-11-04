using ChatApp.Core.DTOs.Response;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Services;
using ChatApp.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.Core.Services
{
    public class TokenClaimService : ITokenClaimService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public TokenClaimService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        public async Task<(string Token, string RawFingerprint)> GenerateJwtToken(string username)
        {
            var user = await _userService.GetUser(username).ConfigureAwait(false);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            // Generate raw fingerprint and hashed fingerprint
            var rawFingerprint = Guid.NewGuid().ToString();
            var hashedFingerprint = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(rawFingerprint)));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Fingerprint", hashedFingerprint) // Add hashed fingerprint to JWT claims
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:ExpirePeriod"]));
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = expires,
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            });

            return (token, rawFingerprint);
        }





        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        async public Task<TokenValidationResult>? GetTokenPrincipal(string token)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var validation = new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidateLifetime = false,
                ValidateActor = false,
                ValidateIssuer = false,
                ValidateAudience = false,
            };
            return await new JsonWebTokenHandler().ValidateTokenAsync(token, validation);
        }
    }
}

