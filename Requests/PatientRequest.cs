using System.ComponentModel.DataAnnotations;
using HealthDeskAPI.Models.Enums;

namespace HealthDeskAPI.Requests;

public record PatientRequest
{
    [Required] public required string Nik { get; init; }
    [Required] public DateOnly DateOfBirth { get; init; }
    [Required] public Gender Gender { get; init; }
    [Required] public required string PhoneNumber { get; init; }
    public string? Address { get; init; }
}