using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Infrastructure.Authentication
{
    public class JwtCustomEvents: JwtBearerEvents
    {
        public override async Task TokenValidated(TokenValidatedContext context)
        {
            //var claims = new[] {
            //    new Claim(ClaimTypes.Name, "username"),
            //            new Claim("ConfidentialAccess", "true")
            //};
            //var identity = new ClaimsIdentity(claims, "JWT");
            //var principal = new ClaimsPrincipal(identity);

            //context.Principal = principal;

            await base.TokenValidated(context);
        }
    }
}
