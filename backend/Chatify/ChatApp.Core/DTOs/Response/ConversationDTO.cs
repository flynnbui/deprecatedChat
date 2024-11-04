namespace ChatApp.Core.DTOs.Response
{
    public class ConversationDTO
    {
        public Guid ContactId { get; set; }  
        public string Title { get; set; }      
        public string SenderUsername { get; set; } 
        public string LatestMessage { get; set; }  
        public DateTime LastUpdated { get; set; } 
    }
}
