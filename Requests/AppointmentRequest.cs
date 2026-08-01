namespace HealthDeskAPI.Requests;

public record AppointmentRequest(
    int PatientId,
    int DoctorId,
    int ScheduleId,
    DateOnly AppointmentDate,
    string Notes
);