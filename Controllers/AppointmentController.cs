using HealthDeskAPI.Models;
using HealthDeskAPI.Models.Enums;
using HealthDeskAPI.Requests;
using HealthDeskAPI.Responses;
using HealthDeskAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthDeskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(HealthDeskApiContext context, QueueNumberGenerator queueNumberGenerator)
        : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentResponse>> GetAppointment(int id)
        {
            var appointment = await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Schedule)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            var response = new AppointmentResponse(
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Status,
                appointment.Schedule.DayOfWeek,
                appointment.Schedule.StartTime,
                appointment.Schedule.EndTime
            );

            return response;
        }
        
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(AppointmentRequest request)
        {
            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                ScheduleId = request.ScheduleId,
                AppointmentDate = request.AppointmentDate,
                Status = AppointmentStatus.Scheduled,
                Notes = request.Notes
            };

            appointment.QueueNumber = await queueNumberGenerator.Generate(appointment);
            
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
        }
    }
}