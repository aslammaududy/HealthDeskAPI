using HealthDeskAPI.Models.Enums;

namespace HealthDeskAPI.Responses;

public record PatientResponse(
    int Id,
    string? MedicalRecordNumber,
    string Nik,
    string Fullname,
    DateOnly DateOfBirth,
    Gender Gender,
    string PhoneNumber,
    string? Address
    );