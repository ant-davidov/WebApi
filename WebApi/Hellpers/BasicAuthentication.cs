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
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No header");
            var headerKey = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (headerKey.Parameter == null) 
                return AuthenticateResult.Fail("Parametr is empty");
            byte[] bytes = Convert.FromBase64String(headerKey.Parameter);
            string credentials = Encoding.UTF8.GetString(bytes);
            if(string.IsNullOrWhiteSpace(credentials)) 
                return AuthenticateResult.Fail("Parametr is empty");

            string[] array = credentials.Split(":");
            string email = array[0];
            string password = array[1];
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
