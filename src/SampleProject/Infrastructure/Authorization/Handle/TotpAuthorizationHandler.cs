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
    public class TotpAuthorizationHandler : AuthorizationHandler<TotpAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TotpAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TotpAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var userIdentity = context.User.Identity;

            // pode ser feito a validação do token, mesmo que valido (não expirado e assinatura correta), esta bloqueado no banco de dados ou algum outro lugar.

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
