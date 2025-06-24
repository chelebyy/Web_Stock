using AutoMapper;
using MediatR;
using Stock.Application.Models.DTOs;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Specifications.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Permissions.Queries.GetPermissionById;

public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPermissionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PermissionDto> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        var spec = new PermissionByIdSpecification(request.Id);
        var permission = await _unitOfWork.GetRepository<Permission>().FirstOrDefaultAsync(spec, cancellationToken);

        if (permission == null)
        {
            throw new NotFoundException($"Permission with ID {request.Id} not found.");
        }

        return _mapper.Map<PermissionDto>(permission);
    }
} 