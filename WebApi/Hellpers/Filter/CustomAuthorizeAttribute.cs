using System.Data;
using System.Net;
using System.Security.Claims;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;

namespace WebApi.Hellpers.Filter
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        private readonly bool _adminPrivileges;
        private readonly string _mask;
        private readonly string _roles;
        public CustomAuthorizeAttribute(bool adminPrivileges = false, string mask = "id", string roles = null)
        {
            _adminPrivileges = adminPrivileges;
            _mask = mask; //this is the name of a variable whose value must match the user id
            _roles = roles;
        }

        public  void OnAuthorization(AuthorizationFilterContext context)
        {

            if (!CheeckRole(_roles, context))
            {
                context.Result = new ForbidResult();
                return;
            }

            if (_adminPrivileges)
            {
                if (! CheeckAdminPrivileges(context, _mask))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            var claim = context.HttpContext.User.Claims.FirstOrDefault();
            if (null == claim)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            if (claim.Type == ClaimTypes.Anonymous && allowAnonymous) return;
            if (claim.Type == ClaimTypes.Name) return;

            context.Result = new UnauthorizedResult();
            return;
        }

        private static bool CheeckRole(string roles, AuthorizationFilterContext context)
        {
            if (roles == null) return true;
            var containsRole = roles.Split(',').Any(role => context.HttpContext.User.IsInRole(role.Trim()));
            return containsRole;
        }

        private  static bool CheeckAdminPrivileges(AuthorizationFilterContext context, string mask)
        {
            var id = Int64.Parse(context.HttpContext.Request.RouteValues[mask] as string);
            var role = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            var idAccount = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();
            if (idAccount == null) return false;
            var intId = Int64.Parse(idAccount);
            if (intId == id || RoleEnum.ADMIN.ToString() == role) return true;
            return false;

        }
    }
}