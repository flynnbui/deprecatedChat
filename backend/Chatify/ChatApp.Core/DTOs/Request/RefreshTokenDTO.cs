using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.DTOs.Request
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
