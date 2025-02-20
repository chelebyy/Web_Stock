using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using StockAPI.Services;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using StockAPI.Models.DTOs;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<UserController> _logger;
    private readonly IHashService _hashService;

    public UserController(
        ApplicationDbContext context,
        IAuditLogService auditLogService,
        ILogger<UserController> logger,
        IHashService hashService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        try
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role.Name,
                    IsAdmin = u.IsAdmin,
                    CreatedAt = u.CreatedAt,
                    LastLoginAt = u.LastLoginAt
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcılar listelenirken hata oluştu");
            return StatusCode(500, new { message = "Kullanıcılar listelenirken bir hata oluştu" });
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try 
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
            {
                _logger.LogWarning("ID'si {Id} olan kullanıcı bulunamadı", id);
                return NotFound();
            }

            return Ok(new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role.Name,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
    {
        ArgumentNullException.ThrowIfNull(createUserDto);

        if (await _context.Users.AnyAsync(u => u.Username == createUserDto.Username))
        {
            return BadRequest("Bu kullanıcı adı zaten kullanılıyor");
        }

        var user = new User
        {
            Username = createUserDto.Username,
            PasswordHash = _hashService.HashPassword(createUserDto.Password),
            IsAdmin = createUserDto.IsAdmin,
            RoleId = createUserDto.RoleId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = (await _context.Roles.FindAsync(user.RoleId))?.Name ?? "User",
            IsAdmin = user.IsAdmin,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userDto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto updateUserDto)
    {
        ArgumentNullException.ThrowIfNull(updateUserDto);

        if (id != updateUserDto.Id)
        {
            return BadRequest("ID'ler eşleşmiyor");
        }

        try
        {
            var existingUser = await _context.Users.FindAsync(id).ConfigureAwait(false);
            if (existingUser == null)
            {
                return NotFound($"ID'si {id} olan kullanıcı bulunamadı");
            }

            var oldUser = new { existingUser.Username, existingUser.IsAdmin, existingUser.RoleId };

            existingUser.Username = updateUserDto.Username;
            existingUser.IsAdmin = updateUserDto.IsAdmin;
            existingUser.RoleId = updateUserDto.RoleId;

            await _context.SaveChangesAsync();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                await _auditLogService.LogAsync<object>(
                    int.Parse(currentUserId),
                    "UpdateUser",
                    id.ToString(),
                    oldUser,
                    new { updateUserDto.Username, updateUserDto.IsAdmin, updateUserDto.RoleId });
            }

            return Ok(new { message = "Kullanıcı başarıyla güncellendi" });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Kullanıcı güncellenirken eşzamanlılık hatası oluştu - ID: {UserId}", id);
            return StatusCode(409, new { message = "Kullanıcı başka bir kullanıcı tarafından güncellenmiş olabilir" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı güncellenirken hata oluştu. ID: {Id}", id);
            throw;
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı" });
            }

            if (user.IsAdmin)
            {
                return BadRequest(new { message = "Admin kullanıcısı silinemez" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                await _auditLogService.LogAsync<User>(
                    int.Parse(currentUserId),
                    "DeleteUser",
                    id.ToString(),
                    user,
                    null);
            }

            return Ok(new { message = "Kullanıcı başarıyla silindi" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı silinirken hata oluştu - ID: {UserId}", id);
            return StatusCode(500, new { message = "Kullanıcı silinirken bir hata oluştu" });
        }
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public int RoleId { get; set; }
    public string Role { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public bool IsAdmin { get; set; }
    public int RoleId { get; set; }
}
