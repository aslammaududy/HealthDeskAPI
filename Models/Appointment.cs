using HealthDeskAPI.Models.Enums;

namespace HealthDeskAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public required string QueueNumber { get; set; }
        public required int PatientId { get; set; }
        public required Patient Patient { get; set; }
        public required int DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
        public required int ScheduleId { get; set; }
        public required Schedule Schedule { get; set; }
        public required DateOnly AppointmentDate { get; set; }
        public required AppointmentStatus Status {get; set; }
        public string? Notes {get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}