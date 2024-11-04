using ChatApp.Core.DTOs.Request;
using ChatApp.Core.DTOs.Response;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Services;
using ChatApp.Core.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;


namespace ChatApp.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserService _userService;
        private readonly ITokenClaimService _tokenClaimsService;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IAuthRepository authRepository, IUserService userService, ITokenClaimService tokenClaimsService, ILogger<AuthService> logger)
        {
            _userService = userService;
            _authRepository = authRepository;
            _tokenClaimsService = tokenClaimsService;
            _logger = logger;
        }

        public async Task<LoginResult> Login(LoginDTO loginRequest)
        {
            var (result, errorMessage) = await _authRepository.LoginAsync(loginRequest.UserName, loginRequest.Password).ConfigureAwait(false);
            var identityUser = await _userService.GetUser(loginRequest.UserName).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(errorMessage);
            }

            var response = new LoginResult();
            response.IsLogedIn = result.Succeeded;

            // Generate the JWT token and retrieve the raw fingerprint
            var (jwtToken, rawFingerprint) = await _tokenClaimsService.GenerateJwtToken(loginRequest.UserName).ConfigureAwait(false);
            response.Token = jwtToken;
            response.RefreshToken = _tokenClaimsService.GenerateRefreshToken();
            response.RawFingerprint = rawFingerprint;

            // Update the user’s refresh token and expiration
            identityUser.RefreshToken = response.RefreshToken;
            identityUser.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);
            await _userService.UpdateUser(identityUser);

            // Return both the response and the raw fingerprint
            return response;
        }



        public async Task<RegisterResponseDTO> Register(RegisterDTO registerRequest)
        {
            var result = await _authRepository.CreateUserAsync(registerRequest.UserName, registerRequest.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("User registration failed: {Errors}", result.Errors.Select(e => e.Description));
                return new RegisterResponseDTO
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };

            }
            return new RegisterResponseDTO { Succeeded = true };
        }

        public async Task<LoginResponseDTO> RefreshToken(RefreshTokenDTO model, string fingerPrint)
        {
            // Extract claims from the expired token (this includes the fingerprint claim)
            var principal = _tokenClaimsService.GetTokenPrincipal(model.Token);

            var response = new LoginResponseDTO();
            if (principal?.Result?.ClaimsIdentity?.Name is null)
            {
                return null;
            }
                
            // Get the user based on the token’s claims
            var identityUser = await _userService.GetUser(principal.Result.ClaimsIdentity.Name);
            if (identityUser == null || identityUser.RefreshToken != model.RefreshToken || identityUser.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            // Hash the raw fingerprint from the cookie
            var hashedFingerprintFromCookie = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(fingerPrint)));

            // Retrieve the hashed fingerprint stored in the sever
            var jwtFingerprint = principal.Result.Claims.FirstOrDefault(c => c.Key == "Fingerprint").Value?.ToString();

            // Compare the fingerprints
            if (hashedFingerprintFromCookie != jwtFingerprint)
            {
                return null; // Return null if fingerprints don't match
            }

            // Generate a new JWT and refresh token if the fingerprint and refresh token are valid

            response.IsLogedIn = true;
            var (jwtToken, rawFingerprint) = await _tokenClaimsService.GenerateJwtToken(identityUser.UserName).ConfigureAwait(false);
            response.Token = jwtToken;
            response.RefreshToken = _tokenClaimsService.GenerateRefreshToken();

            // Update the user's refresh token and expiry time
            identityUser.RefreshToken = response.RefreshToken;
            identityUser.RefreshTokenExpiry = DateTime.UtcNow.AddHours(12);
            await _userService.UpdateUser(identityUser);

            return response;
        }


    }
}
