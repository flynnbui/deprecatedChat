using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Core.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public User Sender { get; set; }
        public Guid RecipientId { get; set; }
        public User Recipient { get; set; }
        public string EncryptedContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
