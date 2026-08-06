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
    [Authorize(Roles = "Superadmin")]
    [ApiController]
    public class UserController : ControllerBase, IMappable<UserResponse, ApplicationUser, UserRequest>
    {
        private readonly HealthDeskApiContext _context;

        public UserController(HealthDeskApiContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _context.ApplicationUsers.ToListAsync();
            return users.Select(ToResponse).ToList();
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return ToResponse(user);
        }

        // PUT: api/User/{id}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserRequest request)
        {
            var applicationUser = await _context.ApplicationUsers.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            UpdateModel(request, applicationUser);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.ApplicationUsers.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }

        public UserResponse ToResponse(ApplicationUser model)
        {
            return new UserResponse(
                model.Id,
                model.Email!,
                model.FirstName,
                model.LastName,
                model.CreatedAt
            );
        }

        public void UpdateModel(UserRequest request, ApplicationUser model)
        {
            model.Email = request.Email;
            model.UserName = request.Email;
            model.FirstName = request.FirstName;
            model.LastName = request.LastName;
        }
    }
}
