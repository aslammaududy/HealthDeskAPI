using HealthDeskAPI.Interfaces;
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
        : ControllerBase, IMappable<AppointmentResponse, Appointment, AppointmentRequest>
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

            return ToResponse(appointment);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentResponse>> PostAppointment(AppointmentRequest request)
        {
            var appointment = new Appointment();
            UpdateModel(request, appointment);

            appointment.QueueNumber = await queueNumberGenerator.Generate(appointment);

            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, ToResponse(appointment));
        }

        public AppointmentResponse ToResponse(Appointment appointment)
        {
            return new AppointmentResponse(
                appointment.Id,
                appointment.Patient.FullName,
                appointment.Doctor.FullName,
                appointment.Status,
                appointment.Schedule.DayOfWeek,
                appointment.Schedule.StartTime,
                appointment.Schedule.EndTime
            );
        }

        public void UpdateModel(AppointmentRequest request, Appointment model)
        {
            model.PatientId = request.PatientId;
            model.DoctorId = request.DoctorId;
            model.ScheduleId = request.ScheduleId;
            model.AppointmentDate = request.AppointmentDate;
            model.Status = AppointmentStatus.Scheduled;
            model.Notes = request.Notes;
        }
    }
}