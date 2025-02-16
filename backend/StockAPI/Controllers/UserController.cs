using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace StockAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly StockContext _context;

        public UserController(StockContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new User 
                { 
                    Id = u.Id,
                    Username = u.Username,
                    IsAdmin = u.IsAdmin,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                })
                .ToListAsync();

            return users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Select(u => new User 
                { 
                    Id = u.Id,
                    Username = u.Username,
                    IsAdmin = u.IsAdmin,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                })
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return BadRequest("Bu kullanıcı adı zaten kullanılıyor");
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var createdUser = new User
            {
                Id = user.Id,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, createdUser);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                existingUser.PasswordHash = HashPassword(user.PasswordHash);
            }

            existingUser.Username = user.Username;
            existingUser.IsAdmin = user.IsAdmin;

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
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Username.ToLower() == "admin")
            {
                return BadRequest("Admin kullanıcısı silinemez");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
