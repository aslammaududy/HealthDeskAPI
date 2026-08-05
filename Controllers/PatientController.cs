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
    public class PatientController : ControllerBase, IMappable<PatientResponse, Patient, PatientRequest>
    {
        private readonly HealthDeskApiContext _context;

        public PatientController(HealthDeskApiContext context)
        {
            _context = context;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponse>>> GetPatients()
        {
            var patients = await _context.Patients.ToListAsync();
            return patients.Select(ToResponse).ToList();
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // PUT: api/Patient/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, PatientRequest request)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            UpdateModel(request, patient);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Patient
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PatientResponse>> PostPatient(PatientRequest request)
        {
            var patient = new Patient();
            UpdateModel(request, patient);

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, ToResponse(patient));
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }

        public PatientResponse ToResponse(Patient patient)
        {
            return new PatientResponse(
                patient.Id,
                patient.MedicalRecordNumber,
                patient.Nik,
                patient.FullName,
                patient.DateOfBirth,
                patient.Gender,
                patient.PhoneNumber,
                patient.Address
            );
        }

        public void UpdateModel(PatientRequest request, Patient model)
        {
            model.Nik = request.Nik;
            model.FullName = request.FullName;
            model.DateOfBirth = request.DateOfBirth;
            model.Gender = request.Gender;
            model.PhoneNumber = request.PhoneNumber;
            model.Address = request.Address;
        }
    }
}