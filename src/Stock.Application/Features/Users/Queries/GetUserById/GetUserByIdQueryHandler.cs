using MediatR;
using Stock.Application.Interfaces; // IUnitOfWork
using Stock.Application.Models.DTOs; // UserDto
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Users; // UserWithRoleAndPermissionsSpecification için
using System.Threading;
using System.Threading.Tasks;
using AutoMapper; // AutoMapper için

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            // Rol ve izin bilgilerini de dahil etmek için uygun specification'ı kullan
            var spec = new UserWithRoleAndPermissionsSpecification(request.Id);

            // IRepository<User> üzerinden veriyi çek
            var user = await _unitOfWork.GetRepository<User>().FirstOrDefaultAsync(spec, cancellationToken);

            if (user == null)
            {
                return null; // Veya NotFoundException fırlatılabilir, duruma göre karar verilir.
            }

            // AutoMapper ile User entity'sini UserDto'ya map et
            return _mapper.Map<UserDto>(user);
        }
    }
} 