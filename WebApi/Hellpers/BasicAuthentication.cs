using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WebApi.Interfaces;

namespace WebApi.Hellpers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUnitOfWork unitOfWork) :base(options, logger, encoder, clock)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(Request.Method.ToLower() == "get" || OriginalPath == "/registration")
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {   var claim = new[] { new Claim(ClaimTypes.Anonymous, "Anonymous")};
                    var identit = new ClaimsIdentity(claim,Scheme.Name);
                    var principa = new ClaimsPrincipal(identit);
                    var ticke = new AuthenticationTicket(principa, Scheme.Name);
                    return AuthenticateResult.Success(ticke); 
                }
            }
           
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No header");
            if(!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out var headerKey)) return AuthenticateResult.Fail("No header value");
            if (String.IsNullOrWhiteSpace(headerKey.Parameter)) 
                return AuthenticateResult.Fail("Parametr is empty");
            byte[] bytes = Convert.FromBase64String(headerKey.Parameter);
            string credentials = Encoding.UTF8.GetString(bytes);
            if(string.IsNullOrWhiteSpace(credentials)) 
                return AuthenticateResult.Fail("Parametr is empty");
            string[] array = credentials.Split(":");
            string email = array[0].Trim();
            string password = array[1].Trim();
            var user = await _unitOfWork.AccountRepository.GetAccountByEmailAndPasswordAsync(email, password);
            if (user == null) 
                return AuthenticateResult.Fail("User or password invalid");
            var claims = new[] { new Claim(ClaimTypes.Name, user.Email)};
            var identity = new ClaimsIdentity(claims,Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);         
        }
    }
}
