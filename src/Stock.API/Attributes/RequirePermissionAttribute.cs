using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stock.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stock.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionAttribute : TypeFilterAttribute
    {
        public RequirePermissionAttribute(string permissionName) : base(typeof(RequirePermissionFilter))
        {
            Arguments = new object[] { permissionName };
        }
    }

    public class RequirePermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permissionName;
        private readonly IPermissionService _permissionService;

        public RequirePermissionFilter(string permissionName, IPermissionService permissionService)
        {
            _permissionName = permissionName;
            _permissionService = permissionService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Kullanıcı kimliği doğrulanmamışsa
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Admin rolü varsa her zaman izin ver
            if (context.HttpContext.User.IsInRole("Admin"))
            {
                return;
            }

            // Token'daki izinleri kontrol et
            var hasPermission = context.HttpContext.User.Claims
                .Where(c => c.Type == "Permission")
                .Any(c => c.Value == _permissionName);

            if (hasPermission)
            {
                return;
            }

            // Veritabanından izinleri kontrol et
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                hasPermission = await _permissionService.HasPermissionAsync(userId, _permissionName);
                if (hasPermission)
                {
                    return;
                }
            }

            // İzin yoksa 403 Forbidden hatası döndür
            context.Result = new ForbidResult();
        }
    }
} 