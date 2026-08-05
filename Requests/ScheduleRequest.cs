using System.ComponentModel.DataAnnotations;

namespace HealthDeskAPI.Requests;

public record ScheduleRequest
{
    [Required] public int? DoctorId { get; set; }
    [Required] public DayOfWeek DayOfWeek { get; set; }
    [Required] public TimeOnly StartTime { get; set; }
    [Required] public TimeOnly EndTime { get; set; }
    [Required] public int MaxQuota { get; set; }
}