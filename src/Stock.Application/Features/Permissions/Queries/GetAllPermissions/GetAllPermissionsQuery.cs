using MediatR;
using Stock.Application.Models.DTOs; // Correct namespace
using System.Collections.Generic;

namespace Stock.Application.Features.Permissions.Queries.GetAllPermissions;

/// <summary>
/// Represents the query to retrieve all permissions.
/// </summary>
public class GetAllPermissionsQuery : IRequest<IEnumerable<PermissionDto>> // Correct DTO reference
{
    // Bu sorgu için parametreye gerek yok
} 