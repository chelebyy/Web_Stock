using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Stock.Application.Common.CQRS;
using Stock.Application.Common.Exceptions;
using Stock.Application.Common.Interfaces;
using Stock.Application.Features.Users.Dtos;
using Stock.Domain.Interfaces;

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// ID'ye göre kullanıcı sorgulama işleyicisi
    /// </summary>
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Yeni bir GetUserByIdQueryHandler örneği oluşturur
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        public GetUserByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Sorguyu işler
        /// </summary>
        /// <param name="query">İşlenecek sorgu</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Kullanıcı bilgileri</returns>
        public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"ID'ye göre kullanıcı sorgulanıyor: {query.Id}");

            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(query.Id);
            if (user == null)
            {
                _logger.LogWarn($"Kullanıcı bulunamadı. ID: {query.Id}");
                throw new NotFoundException("Kullanıcı", query.Id);
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
} 