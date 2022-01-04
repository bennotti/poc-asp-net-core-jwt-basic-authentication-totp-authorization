using Microsoft.AspNetCore.Authorization;
using SampleProject.Infrastructure.Authorization.Extensions;
using SampleProject.Infrastructure.Authorization.Requirements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Infrastructure.Authorization.Handle
{
    public class JwtAuthorizationHandler : AuthorizationHandler<JwtAuthorizationRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var userIdentity = context.User.Identity;
            if (!userIdentity.IsJwtAuthenticate())
            {
                return;
            }

            // pode ser feito a validação do token, mesmo que valido (não expirado e assinatura correta), esta bloqueado no banco de dados ou algum outro lugar.

            context.Succeed(requirement);

            await Task.CompletedTask;
        }
    }
}
