using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record UserRequest
{
    [Required] public required string Email { get; init; }
    [Required] public required string FirstName { get; init; }
    [Required] public required string LastName { get; init; }
}
