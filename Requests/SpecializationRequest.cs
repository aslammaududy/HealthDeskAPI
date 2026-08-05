using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record SpecializationRequest
{
    [Required] public required string Code { get; init; }
    [Required] public required string Name { get; init; }
}