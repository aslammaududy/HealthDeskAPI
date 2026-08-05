using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record DoctorRequest
{
    [Required]
    public required string FullName { get; init; }
    [Required]
    public int SpecializationId { get; init; }
    [Required]
    public bool IsActive { get; init; }
}