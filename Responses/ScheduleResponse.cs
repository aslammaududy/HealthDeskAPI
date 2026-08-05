namespace HealthDeskAPI.Responses;

public record ScheduleResponse(
    int Id,
    string? DoctorName,
    DayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int MaxQuota
);