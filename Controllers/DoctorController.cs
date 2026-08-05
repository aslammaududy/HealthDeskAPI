using HealthDeskAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthDeskAPI.Models;
using HealthDeskAPI.Requests;
using HealthDeskAPI.Responses;

namespace HealthDeskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase, IMappable<DoctorResponse, Doctor, DoctorRequest>
    {
        private readonly HealthDeskApiContext _context;

        public DoctorController(HealthDeskApiContext context)
        {
            _context = context;
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorResponse>>> GetDoctors()
        {
            var doctors = await _context.Doctors.Include(d => d.Specialization).ToListAsync();

            return doctors.Select(ToResponse).ToList();
        }

        // GET: api/Doctor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorResponse>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.Include(d => d.Specialization).FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return ToResponse(doctor);
        }

        // PUT: api/Doctor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, DoctorRequest request)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            UpdateModel(request, doctor);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Doctor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DoctorResponse>> PostDoctor(DoctorRequest request)
        {
            var doctor = new Doctor();
            UpdateModel(request, doctor);

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, ToResponse(doctor));
        }

        // DELETE: api/Doctor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.Id == id);
        }

        public DoctorResponse ToResponse(Doctor doctor)
        {
            return new DoctorResponse(
                doctor.Id,
                doctor.FullName,
                doctor.SpecializationId,
                doctor.Specialization?.Name,
                doctor.IsActive
            );
        }

        public void UpdateModel(DoctorRequest request, Doctor model)
        {
            model.FullName = request.FullName;
            model.SpecializationId = request.SpecializationId;
            model.IsActive = request.IsActive;
        }
    }
}