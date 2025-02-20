using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using StockAPI.Models.DTOs;

namespace StockAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolesController> _logger;

        public RolesController(ApplicationDbContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleWithUsersDto>>> GetRoles()
        {
            _logger.LogInformation("Tüm roller ve kullanıcıları getiriliyor");
            
            var roles = await _context.Roles
                .Include(r => r.Users)
                .Where(r => !r.IsDeleted)
                .Select(r => new RoleWithUsersDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    Users = r.Users.Where(u => !u.IsDeleted).Select(u => new UserDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        CreatedAt = u.CreatedAt
                    }).ToList()
                })
                .ToListAsync();

            return roles;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            role.CreatedAt = DateTime.UtcNow;
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            var existingRole = await _context.Roles.FindAsync(id);
            if (existingRole == null || existingRole.IsDeleted)
            {
                return NotFound();
            }

            existingRole.Name = role.Name;
            existingRole.Description = role.Description;
            existingRole.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null || role.IsDeleted)
            {
                return NotFound();
            }

            role.IsDeleted = true;
            role.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id && !e.IsDeleted);
        }
    }

    public class RoleWithUsersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserDto> Users { get; set; }
    }
} 