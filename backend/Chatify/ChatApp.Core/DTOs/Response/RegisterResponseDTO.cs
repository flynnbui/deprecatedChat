using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.DTOs.Response
{
    public class RegisterResponseDTO
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
