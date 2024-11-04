using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Models
{
    public class LoginResult
    {
        public bool IsLogedIn { get; set; } = false;
        public string Token { get; set; }
        public string RefreshToken { get; internal set; }

        public string RawFingerprint { get; set; }
    }
}