using HealthDeskAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthDeskAPI.Models;
using HealthDeskAPI.Requests;
using HealthDeskAPI.Responses;
using Microsoft.AspNetCore.Authorization;

namespace HealthDeskAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Superadmin,Registration")]
    [ApiController]
    public class ScheduleController : ControllerBase, IMappable<ScheduleResponse, Schedule, ScheduleRequest>
    {
        private readonly HealthDeskApiContext _context;

        public ScheduleController(HealthDeskApiContext context)
        {
            _context = context;
        }

        // GET: api/Schedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleResponse>>> GetSchedules()
        {
            var schedules = await _context.Schedules.ToListAsync();
            return schedules.Select(ToResponse).ToList();
        }

        // GET: api/Schedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleResponse>> GetSchedule(int id)
        {
            var schedule = await _context.Schedules.Include(s => s.Doctor).FirstOrDefaultAsync(s => s.Id == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return ToResponse(schedule);
        }

        // PUT: api/Schedule/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleRequest request)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            UpdateModel(request, schedule);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduleExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Schedule
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ScheduleResponse>> PostSchedule(ScheduleRequest request)
        {
            var schedule = new Schedule();
            UpdateModel(request, schedule);

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, ToResponse(schedule));
        }

        // DELETE: api/Schedule/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }

        public ScheduleResponse ToResponse(Schedule schedule)
        {
            return new ScheduleResponse(
                schedule.Id,
                schedule.Doctor?.FullName,
                schedule.DayOfWeek,
                schedule.StartTime,
                schedule.EndTime,
                schedule.MaxQuota
            );
        }

        public void UpdateModel(ScheduleRequest request, Schedule model)
        {
            model.DayOfWeek = request.DayOfWeek;
            model.DoctorId = request.DoctorId;
            model.StartTime = request.StartTime;
            model.EndTime = request.EndTime;
            model.MaxQuota = request.MaxQuota;
        }
    }
}