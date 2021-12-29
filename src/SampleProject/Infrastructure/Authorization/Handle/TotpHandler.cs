using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SampleProject.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Infrastructure.Authorization.Handle
{
    public class TotpHandler : AuthorizationHandler<TotpRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TotpHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TotpRequirement requirement)
        {
            if (_httpContextAccessor == null)
            {
                context.Fail();
                return;
            }
            var secret = _httpContextAccessor.HttpContext.Request.Headers["OTP-TEMP-SECRET"].FirstOrDefault()?.Split(" ").Last();
            var totpToken = _httpContextAccessor.HttpContext.Request.Headers["OTP-USER-TOKEN"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(totpToken))
            {
                context.Fail();
                return;
            }

            if (TotpHelper.Validar(secret, totpToken))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();

            await Task.CompletedTask;
        }
    }
}
