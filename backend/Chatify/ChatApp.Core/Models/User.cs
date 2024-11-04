using Microsoft.AspNetCore.Identity;

namespace ChatApp.Core.Models
{
    // User model class, we can modify this if we need
    // to add custom properties to the user
    public class User : IdentityUser<Guid> {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set;}
    }
}
