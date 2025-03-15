using MediatR;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;

namespace Stock.Application.Features.Users.Queries.GetPaginatedUsers
{
    public class GetPaginatedUsersQuery : IRequest<PaginatedResult<UserDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        
        // Filtreleme özellikleri
        public string UsernameFilter { get; set; }
        public string SicilFilter { get; set; }
        public int? RoleIdFilter { get; set; }
        public bool? IsActiveFilter { get; set; }
        public bool? IsAdminFilter { get; set; }
        public string SortBy { get; set; } = "Username"; // Varsayılan sıralama alanı
        public bool SortAscending { get; set; } = true; // Varsayılan sıralama yönü (artan)
    }

    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
} 