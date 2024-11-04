namespace ChatApp.Core.DTOs.Response
{
    public class LoginResponseDTO
    {
        public bool IsLogedIn { get; set; } = false;
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
