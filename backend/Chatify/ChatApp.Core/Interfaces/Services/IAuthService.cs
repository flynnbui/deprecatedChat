using ChatApp.Core.DTOs.Request;
using ChatApp.Core.DTOs.Response;
using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;


namespace ChatApp.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDTO> Register(RegisterDTO registerRequest);
        Task<LoginResult> Login(LoginDTO loginRequest);
        Task<LoginResponseDTO> RefreshToken(RefreshTokenDTO model, string fingerPrint);
    }
}
