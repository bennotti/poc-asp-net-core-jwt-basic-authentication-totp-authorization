using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OtpNet;
using SampleProject.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleProject.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TotpController : ControllerBase
    {
        private readonly ILogger<TotpController> _logger;

        public TotpController(ILogger<TotpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public TotpKeyResponseDto ObterTotpKey()
        {
            _logger.LogInformation("Gerando chave TOTP Key");
            var otpToken = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
            return new TotpKeyResponseDto
            {
                Key = otpToken,
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
