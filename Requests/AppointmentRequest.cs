using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record AppointmentRequest
{
    [Required] public int PatientId { get; init; }
    [Required] public int DoctorId { get; init; }
    [Required] public int ScheduleId { get; init; }
    [Required] public DateOnly AppointmentDate { get; init; }
    public string? Notes { get; init; }
}
