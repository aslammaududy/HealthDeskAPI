using Microsoft.AspNetCore.Identity;

namespace HealthDeskAPI.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } =  string.Empty;
        public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}