using HealthDeskAPI.Models.Enums;

namespace HealthDeskAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int QueueNumber { get; set; }
        public required int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public required int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public required int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = null!;
        public required DateOnly AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}