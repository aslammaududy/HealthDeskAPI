using HealthDeskAPI.Models.Enums;
using DayOfWeek = System.DayOfWeek;

namespace HealthDeskAPI.Responses;

public record AppointmentResponse(
    int Id,
    string PatientName,
    string DoctorName,
    AppointmentStatus Status,
    DayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime
    );