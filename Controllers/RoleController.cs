using HealthDeskAPI.Models;
using HealthDeskAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthDeskAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Superadmin")]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    // GET: api/Role
    [HttpGet]
    public ActionResult<IEnumerable<string>> GetRoles()
    {
        return Ok(_roleManager.Roles.Select(r => r.Name).ToList());
    }

    // POST: api/Role
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
    {
        var existingRole = await _roleManager.FindByNameAsync(request.Role);
        if (existingRole != null)
        {
            return Conflict(new { message = $"Role '{request.Role}' already exists." });
        }

        var result = await _roleManager.CreateAsync(new IdentityRole(request.Role));

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetRoles), new { name = request.Role }, new { role = request.Role });
    }

    // DELETE: api/Role/{name}
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteRole(string name)
    {
        var role = await _roleManager.FindByNameAsync(name);
        if (role == null)
        {
            return NotFound(new { message = $"Role '{name}' not found." });
        }

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }

    // GET: api/Role/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = $"User '{userId}' not found." });
        }

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    // POST: api/Role/user/{userId}
    [HttpPost("user/{userId}")]
    public async Task<IActionResult> AddUserRole(string userId, [FromBody] RoleRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = $"User '{userId}' not found." });
        }

        var role = await _roleManager.FindByNameAsync(request.Role);
        if (role == null)
        {
            return NotFound(new { message = $"Role '{request.Role}' not found." });
        }

        var isInRole = await _userManager.IsInRoleAsync(user, request.Role);
        if (isInRole)
        {
            return Conflict(new { message = $"User '{userId}' already has role '{request.Role}'." });
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = $"Role '{request.Role}' added to user '{userId}'." });
    }

    // DELETE: api/Role/user/{userId}/{role}
    [HttpDelete("user/{userId}/{role}")]
    public async Task<IActionResult> RemoveUserRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = $"User '{userId}' not found." });
        }

        var isInRole = await _userManager.IsInRoleAsync(user, role);
        if (!isInRole)
        {
            return NotFound(new { message = $"User '{userId}' does not have role '{role}'." });
        }

        var result = await _userManager.RemoveFromRoleAsync(user, role);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }
}
