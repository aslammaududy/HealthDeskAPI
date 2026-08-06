using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthDeskAPI.Models;
using HealthDeskAPI.Requests;
using Microsoft.AspNetCore.Authorization;

namespace HealthDeskAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Superadmin,Registration")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly HealthDeskApiContext _context;

        public SpecializationController(HealthDeskApiContext context)
        {
            _context = context;
        }

        // GET: api/Specialization
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations()
        {
            return await _context.Specializations.ToListAsync();
        }

        // GET: api/Specialization/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }

        // PUT: api/Specialization/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialization(int id, SpecializationRequest request)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            specialization.Code = request.Code;
            specialization.Name = request.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecializationExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Specialization
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Specialization>> PostSpecialization(SpecializationRequest request)
        {
            var specialization = new Specialization
            {
                Code = request.Code,
                Name = request.Name
            };
            _context.Specializations.Add(specialization);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSpecialization), new { id = specialization.Id }, specialization);
        }

        // DELETE: api/Specialization/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpecialization(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.Id == id);
        }
    }
}
