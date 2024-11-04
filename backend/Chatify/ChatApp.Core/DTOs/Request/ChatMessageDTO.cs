using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.DTOs.Request
{
    public class ChatMessageDTO
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}
