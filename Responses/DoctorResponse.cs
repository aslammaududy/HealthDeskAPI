namespace HealthDeskAPI.Responses;

public record DoctorResponse(
    int Id,
    string FullName,
    int SpecializationId,
    string? SpecializationName,
    bool IsActive
);