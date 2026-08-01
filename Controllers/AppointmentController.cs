using HealthDeskAPI.Models;
using HealthDeskAPI.Models.Enums;
using HealthDeskAPI.Requests;
using HealthDeskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthDeskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(HealthDeskApiContext context, QueueNumberGenerator queueNumberGenerator)
        : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
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