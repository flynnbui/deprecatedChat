using ChatApp.Core.DTOs.Response;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Services;
using ChatApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserService _userService;

        public MessageService(IMessageRepository messageRepository, IUserService userService)
        {
            _messageRepository = messageRepository;
            _userService = userService;
        }

        public async Task AddMessageAsync(Guid senderId, Guid recipientId, string encryptedMessage)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = senderId,
                RecipientId = recipientId,
                EncryptedContent = encryptedMessage,
                Timestamp = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);
        }

        public async Task<IEnumerable<MessageDto>> GetConversationMessagesAsync(Guid userId, Guid contactId)
        {
            var messages = await _messageRepository.GetMessagesAsync(userId, contactId);

            return messages.Select(m => new MessageDto
            {
                MessageId = m.Id,
                SenderId = m.SenderId,
                EncryptedContent = m.EncryptedContent,
                Timestamp = m.Timestamp
            });
        }

        public async Task<IEnumerable<ConversationDTO>> GetRecentConversationsAsync(Guid userId)
        {
            var conversations = await _messageRepository.GetRecentConversationsAsync(userId);

            var conversationDtos = new List<ConversationDTO>();

            foreach (var group in conversations.GroupBy(m => m.SenderId == userId ? m.RecipientId : m.SenderId))
            {
                var contactId = group.Key;
                var lastMessage = group.OrderByDescending(m => m.Timestamp).First();
                var contactUser = await _userService.GetUser(contactId);

                conversationDtos.Add(new ConversationDTO
                {
                    ContactId = contactId,
                    Title = contactUser.UserName,
                    SenderUsername = lastMessage.SenderId == userId ? "You" : contactUser.UserName,
                    LatestMessage = lastMessage.EncryptedContent,
                    LastUpdated = lastMessage.Timestamp
                });
            }

            return conversationDtos.OrderByDescending(c => c.LastUpdated);
        }
    }
}
