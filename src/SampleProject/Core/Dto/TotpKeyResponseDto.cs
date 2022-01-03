using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.Domain.Dto
{
    public class TotpKeyResponseDto
    {
        public string Key { get; set; }
        public string Schema { get; set; }
        public string AccessToken { get; set; }
    }
}
