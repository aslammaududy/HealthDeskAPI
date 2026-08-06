using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record RegisterRequest
{
    [Required]public  string Email { get; init; }
    [Required] public string Password { get; init; }
    [Required] public string FirstName { get; init; }
    [Required] public string LastName { get; init; }
};