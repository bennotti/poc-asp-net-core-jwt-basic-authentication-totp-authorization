using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SampleProject.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Infrastructure.Authorization
{
    public class TotpRequirement : IAuthorizationRequirement
    {
        
    }
}
