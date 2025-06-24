using MediatR;
using Stock.Application.Interfaces; // IUnitOfWork
using Stock.Application.Models.DTOs; // UserDto
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Users; // UsersWithRolesSpecification için
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper; // AutoMapper için
using Stock.Domain.Specifications; // Eklendi

namespace Stock.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserListItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserListItemDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            // Tüm kullanıcıları rolleriyle birlikte getirmek için uygun specification'ı kullan
            // İleride filtreleme/sayfalama için parametreler eklenebilir.
            var spec = new UsersWithRolesSpecification();

            var users = await _unitOfWork.GetRepository<User>().ListAsync(spec, cancellationToken);

            // AutoMapper ile User listesini UserListItemDto listesine map et
            return _mapper.Map<IEnumerable<UserListItemDto>>(users);
        }
    }
} 