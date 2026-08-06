using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record RefreshTokenRequest
{
    [Required] public string Token { get; init; }
    [Required] public string RefreshToken { get; init; }
};
