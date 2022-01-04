using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Infrastructure.Authorization.Handle
{
    public class ValidIdentityAuthorizationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userIdentity = context.User.Identity;

            // validar se o identity esta em formato esperado com as clains minimas

            return Task.CompletedTask;
        }
    }
}
