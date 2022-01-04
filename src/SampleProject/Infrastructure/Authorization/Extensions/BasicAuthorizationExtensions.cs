using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace SampleProject.Infrastructure.Authorization.Extensions
{
    public static class BasicAuthorizationExtensions
    {
        public static bool IsBasicAuthenticate(this IIdentity identity)
        {
            return identity != null && (identity.AuthenticationType ?? "").ToLower().Equals("basic");
        }
    }
}
