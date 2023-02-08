using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WebApi.Interfaces;

namespace WebApi.Hellpers.Filter
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUnitOfWork unitOfWork) : base(options, logger, encoder, clock)
        {
            _unitOfWork = unitOfWork;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
           try 
           {
                
                var header = Request.Headers["Authorization"];
                if (header.Count == 0)
                    return CreateAnonymousTicket();
                if (!AuthenticationHeaderValue.TryParse(header, out var headerKey))
                    return AuthenticateResult.Fail("No header value");
                if (string.IsNullOrWhiteSpace(headerKey.Parameter))
                    return AuthenticateResult.Fail("Parametr is empty");
                byte[] bytes = Convert.FromBase64String(headerKey.Parameter);
                string credentials = Encoding.UTF8.GetString(bytes);
                if (string.IsNullOrWhiteSpace(credentials))
                    return AuthenticateResult.Fail("Parametr is empty");
                string[] array = credentials.Split(":");
                string email = array[0].Trim();
                string password = array[1].Trim();
                var user = await _unitOfWork.AccountRepository.GetAccountByEmailAndPasswordAsync(email, password);
                if (user == null)
                    return AuthenticateResult.Fail("User or password invalid");
                var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch 
            {
                return AuthenticateResult.Fail("Error from convert base 64");
            }

        }
        private AuthenticateResult CreateAnonymousTicket()
        {
            var claim = new[] { new Claim(ClaimTypes.Anonymous, "Anonymous") };
            var identit = new ClaimsIdentity(claim, Scheme.Name);
            var principa = new ClaimsPrincipal(identit);
            var ticke = new AuthenticationTicket(principa, Scheme.Name);
            return AuthenticateResult.Success(ticke);
        }
    }
}
