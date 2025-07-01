using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain;
using Stock.Application.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Application;
using Stock.Domain.Common;
using Stock.Domain.Specifications;
using Stock.Domain.ValueObjects;

namespace Stock.Application.Features.Auth;

public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUnitOfWork unitOfWork, ILogger<AuthService> logger, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        var spec = new UserBySicilSpecification(registerDto.Sicil);
        var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(spec);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration attempt with existing Sicil: {Sicil}", registerDto.Sicil);
            return Result<AuthResponseDto>.Failure(ErrorMessages.SicilAlreadyExists);
        }

        _logger.LogInformation("Creating new user with Sicil: {Sicil}", registerDto.Sicil);

        var passwordHash = _passwordHasher.HashPassword(registerDto.Password);
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result<AuthResponseDto>.Failure(DomainErrors.User.PasswordHashEmpty);
        }

        bool isAdmin = false;
        int? roleId = registerDto.RoleId;
        if (roleId.HasValue)
        {
            var roleSpec = new RoleByIdSpecification(roleId.Value);
            var role = await _unitOfWork.GetRepository<Role>().FirstOrDefaultAsync(roleSpec);
            if (role != null && role.Name.Value == "Admin")
            {
                isAdmin = true;
            }
            else if (role == null)
            {
                _logger.LogWarning("Specified RoleId {RoleId} does not exist during registration for Sicil {Sicil}. Defaulting IsAdmin to false.", roleId.Value, registerDto.Sicil);
                roleId = null;
            }
        }

        var fullNameResult = FullName.Create(registerDto.Adi, registerDto.Soyadi);
        if (!fullNameResult.IsSuccess)
        {
            _logger.LogWarning("FullName creation failed: {Error}", fullNameResult.Error);
            return Result<AuthResponseDto>.Failure(fullNameResult.Error);
        }

        var sicilResult = Sicil.Create(registerDto.Sicil);
        if (!sicilResult.IsSuccess)
        {
            _logger.LogWarning("Sicil creation failed: {Error}", sicilResult.Error);
            return Result<AuthResponseDto>.Failure(sicilResult.Error);
        }

        var userResult = User.Create(
            fullNameResult.Value,
            sicilResult.Value,
            passwordHash,
            roleId,
            isAdmin);

        if (!userResult.IsSuccess)
        {
            _logger.LogWarning("User creation failed: {Error}", userResult.Error);
            return Result<AuthResponseDto>.Failure(userResult.Error);
        }

        var user = userResult.Value;
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("User registered successfully: Sicil {Sicil}, ID {UserId}", user.Sicil, user.Id);

        var registeredUserWithRoleSpec = new UsersWithRolesSpecification(user.Id);
        var registeredUserWithRole = await _unitOfWork.Users.FirstOrDefaultAsync(registeredUserWithRoleSpec);

        if (registeredUserWithRole == null)
        {
            _logger.LogError("Failed to retrieve newly registered user with role: ID {UserId}", user.Id);
            return Result<AuthResponseDto>.Failure("Kullanıcı kaydı sonrası getirilemedi.");
        }

        var token = _jwtTokenGenerator.GenerateToken(registeredUserWithRole);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            Token = token,
            UserId = registeredUserWithRole.Id.ToString(),
            Sicil = registeredUserWithRole.Sicil,
            Adi = registeredUserWithRole.FullName.Adi,
            Soyadi = registeredUserWithRole.FullName.Soyadi,
            IsAdmin = registeredUserWithRole.IsAdmin,
            Success = true,
            RoleId = registeredUserWithRole.RoleId,
            RoleName = registeredUserWithRole.Role?.Name.Value
        });
    }
} 