using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SampleProject.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SampleProject.Infrastructure.Authentication.Handle
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Context.GetEndpoint();
            if (endpoint == null || endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null) return AuthenticateResult.NoResult();

            if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() == null) return AuthenticateResult.NoResult();

            if (Context.User.Identity.IsAuthenticated) {
                return AuthenticateResult.NoResult();
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var schema = authHeader.Scheme;
            var authorization = authHeader.Parameter?.Split(" ").Last();
            if (string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.Fail("Invalid Authorization");
            }

            if (schema.ToLower() != "basic")
            {
                return AuthenticateResult.NoResult();
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, "username"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            await Task.CompletedTask;
            return AuthenticateResult.Success(ticket);
        }
    }
}
