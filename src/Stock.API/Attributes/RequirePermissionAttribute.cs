using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stock.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequirePermissionAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _permission;

        public RequirePermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var permissionService = context.HttpContext.RequestServices
                .GetRequiredService<IPermissionService>();
            var currentUserService = context.HttpContext.RequestServices
                .GetRequiredService<ICurrentUserService>();

            if (currentUserService.UserId == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool hasPermission = await permissionService.UserHasPermissionAsync(
                currentUserService.UserId.Value, _permission);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
                return;
            }

            await next();
        }
    }
} 