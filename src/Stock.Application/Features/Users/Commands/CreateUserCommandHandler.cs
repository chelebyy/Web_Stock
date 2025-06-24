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

namespace Stock.Application.Features.Users.Commands
{
    /// <summary>
    /// Yeni bir kullanıcı oluşturmak için gelen komutu işleyen handler sınıfı.
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILoggerManager<CreateUserCommandHandler> _logger;

        /// <summary>
        /// CreateUserCommandHandler sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="mapper">AutoMapper arayüzü.</param>
        /// <param name="unitOfWork">Unit of Work arayüzü.</param>
        /// <param name="passwordHasher">Şifre hashleme arayüzü.</param>
        /// <param name="logger">Loglama arayüzü.</param>
        public CreateUserCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ILoggerManager<CreateUserCommandHandler> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// CreateUserCommand tipindeki isteği işler ve yeni bir kullanıcı oluşturur.
        /// </summary>
        /// <param name="request">Kullanıcı oluşturma komutu.</param>
        /// <param name="cancellationToken">İptal jetonu.</param>
        /// <returns>Oluşturulan kullanıcının DTO nesnesi.</returns>
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo(LogMessages.CreatingUserWithSicil, request.Sicil);

                // Şifre hashleme
                var passwordHash = _passwordHasher.HashPassword(request.Password);

                // DDD approach: Kullanıcı oluşturmak için factory metodu kullanma
                var userResult = User.Create(
                    request.Adi,
                    request.Soyadi,
                    request.Sicil,
                    passwordHash,
                    request.RoleId,
                    request.IsAdmin);

                if (!userResult.IsSuccess)
                {
                    _logger.LogWarn(LogMessages.InvalidUserData, userResult.Error);
                    throw new InvalidOperationException(userResult.Error);
                }

                var user = userResult.Value;
                
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInfo(LogMessages.UserCreatedWithSicil, user.Id, user.Sicil);

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorCreatingUserWithSicil, request.Sicil);
                throw;
            }
        }
    }
} 