using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record RoleRequest
{
    [Required] public required string Role { get; init; }
}
