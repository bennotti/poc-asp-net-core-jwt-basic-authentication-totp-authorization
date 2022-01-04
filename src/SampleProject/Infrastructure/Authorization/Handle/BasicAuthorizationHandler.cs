using Microsoft.AspNetCore.Authorization;
using SampleProject.Infrastructure.Authorization.Extensions;
using SampleProject.Infrastructure.Authorization.Requirements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Infrastructure.Authorization.Handle
{
    public class BasicAuthorizationHandler : AuthorizationHandler<BasicAuthorizationRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BasicAuthorizationRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated) {
                context.Fail();
                return;
            }

            var userIdentity = context.User.Identity;
            if (!userIdentity.IsBasicAuthenticate()) {
                return;
            }

            context.Succeed(requirement);

            await Task.CompletedTask;
        }
    }
}
