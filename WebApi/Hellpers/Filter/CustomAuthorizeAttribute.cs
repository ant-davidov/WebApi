using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Hellpers.Filter
{
   
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if(context.HttpContext.Request.Method == "GET" && context.HttpContext.User.Identity.IsAuthenticated) return;
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
    }
}