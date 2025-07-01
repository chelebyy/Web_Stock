using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Application.Constants;
using Stock.Application.Common.Interfaces;

using Stock.Domain.Common;
using Stock.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Yeni bir kullanıcı oluşturmak için gelen komutu işleyen handler sınıfı.
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICacheService _cacheService;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private const string CacheKey = "users_all";

        /// <summary>
        /// CreateUserCommandHandler sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="unitOfWork">Unit of Work arayüzü.</param>
        /// <param name="mapper">AutoMapper arayüzü.</param>
        /// <param name="passwordHasher">Şifre hashleme arayüzü.</param>
        /// <param name="cacheService">Cache servisi.</param>
        /// <param name="logger">Loglama arayüzü.</param>
        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            ICacheService cacheService,
            ILogger<CreateUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _cacheService = cacheService;
            _logger = logger;
        }

        /// <summary>
        /// CreateUserCommand tipindeki isteği işler ve yeni bir kullanıcı oluşturur.
        /// </summary>
        /// <param name="request">Kullanıcı oluşturma komutu.</param>
        /// <param name="cancellationToken">İptal jetonu.</param>
        /// <returns>Oluşturulan kullanıcının DTO nesnesi.</returns>
        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(LogMessages.CreatingUserWithSicil, request.Sicil);

                // 1. Gelen isteği Value Object'lere dönüştür
                var fullNameResult = FullName.Create(request.Adi, request.Soyadi);
                var sicilResult = Sicil.Create(request.Sicil);

                // 2. Parola hash'ini oluştur
                var passwordHash = _passwordHasher.HashPassword(request.Password);

                // 3. Value Object ve diğer bilgileri birleştirerek ana entity'i oluştur
                // Her Result'ı ayrı ayrı kontrol et
                if (fullNameResult.IsFailure)
                {
                    return Result<UserDto>.Failure(fullNameResult.Error);
                }

                if (sicilResult.IsFailure)
                {
                    return Result<UserDto>.Failure(sicilResult.Error);
                }

                var userResult = User.Create(
                    fullNameResult.Value,
                    sicilResult.Value,
                    passwordHash,
                    request.RoleId,
                    request.IsAdmin);

                if (userResult.IsFailure)
                {
                    return Result<UserDto>.Failure(userResult.Error);
                }

                var user = userResult.Value;
                
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Invalidate all user list caches
                await _cacheService.RemoveByPrefixAsync("users_page");

                _logger.LogInformation(LogMessages.UserCreatedWithSicil, user.Id, user.Sicil);

                return Result<UserDto>.Success(_mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorCreatingUserWithSicil, request.Sicil);
                return Result<UserDto>.Failure(ex.Message);
            }
        }
    }
} 