namespace ChatApp.Core.DTOs.Response
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid SenderId { get; set; }
        public string EncryptedContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
