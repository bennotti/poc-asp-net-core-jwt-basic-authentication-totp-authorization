using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OtpNet;
using SampleProject.Domain.Dto;
using SampleProject.Infrastructure.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TotpController : ControllerBase
    {
        private readonly ILogger<TotpController> _logger;
        private readonly JwtBuilder _jwtBuilder;

        public TotpController(ILogger<TotpController> logger,
            JwtBuilder jwtBuilder)
        {
            _logger = logger;
            _jwtBuilder = jwtBuilder;
        }

        [HttpGet]
        [Route("")]
        public TotpKeyResponseDto ObterTotpKey()
        {
            _logger.LogInformation("Gerando chave TOTP Key");
            var otpToken = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
            var jwtBuilder = _jwtBuilder.AddClaim(ClaimTypes.Sid, "VALOR_SID");

            var tokenString = jwtBuilder.BuildToken();
            return new TotpKeyResponseDto
            {
                Key = otpToken,
                Schema = "Bearer",
                AccessToken = tokenString,
            };
        }

        [HttpPost]
        [Route("validate")]
        [Authorize(Policy = "TotpValidate")]
        public TotpKeyValidateResponseDto PostValidate()
        {
            _logger.LogInformation("Totp validado");
            return new TotpKeyValidateResponseDto
            { 
                Msg = "Totp validado!"
            };
        }
    }
}
