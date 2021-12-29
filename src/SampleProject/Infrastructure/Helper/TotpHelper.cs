using OtpNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Infrastructure.Helper
{
    public class TotpHelper
    {
        public static bool Validar(string secret, string valor)
        {
            var totp = new Totp(Base32Encoding.ToBytes(secret));
            return totp.VerifyTotp(valor, out _, new VerificationWindow(previous: 1, future: 1));
        }
    }
}
