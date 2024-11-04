using ChatApp.Core.DTOs.Response;

namespace ChatApp.Core.Interfaces.Services
{
    public interface IMessageService
    {
        Task AddMessageAsync(Guid senderId, Guid recipientId, string encryptedMessage);
        Task<IEnumerable<MessageDto>> GetConversationMessagesAsync(Guid userId, Guid contactId);
        Task<IEnumerable<ConversationDTO>> GetRecentConversationsAsync(Guid userId);
    }
}
