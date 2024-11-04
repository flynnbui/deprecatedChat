using ChatApp.Core.DTOs.Request;
using ChatApp.Core.DTOs.Response;
using ChatApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService userServices)
    {
        _authService = userServices;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login(LoginDTO loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var loginResult = await _authService.Login(loginRequest).ConfigureAwait(false);

        if (loginResult.IsLogedIn)
        {
            // Set the raw fingerprint in a secure, HttpOnly cookie
            HttpContext.Response.Cookies.Append("Fingerprint", loginResult.RawFingerprint, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            // Return the login result (token and refresh token)
            return Ok(new LoginResponseDTO {IsLogedIn = true, Token = loginResult.Token, RefreshToken = loginResult.RefreshToken });
        }
        return Unauthorized();
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterDTO registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authService.Register(registerRequest).ConfigureAwait(false);
        if (result.Succeeded)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }


    [HttpPost("refreshtoken")]
    public async Task<ActionResult<LoginResponseDTO>> RefreshToken(RefreshTokenDTO refreshTokenRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Retrieve the raw fingerprint from the secure cookie
        var rawFingerprint = Request.Cookies["Fingerprint"];
        if (rawFingerprint == null)
        {
            return Unauthorized("Fingerprint cookie is missing.");
        }

        // Call the RefreshToken service method and pass the raw fingerprint for validation
        var result = await _authService.RefreshToken(refreshTokenRequest, rawFingerprint).ConfigureAwait(false);
        if (result is null || !result.IsLogedIn)
        {
            return Unauthorized();
        }

        return Ok(new { Token = result.Token, RefreshToken = result.RefreshToken });
    }

}
