using ChatApp.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;


namespace ChatApp.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Method to send a message to a specific user
        public async Task SendMessage(Guid recipientId, string encryptedMessage)
        {
            var senderIdString = ClaimTypes.NameIdentifier;
            if (!Guid.TryParse(senderIdString, out Guid senderId))
            {
                throw new HubException("Invalid sender ID.");
            }

            // Save the encrypted message to the database
            await _messageService.AddMessageAsync(senderId, recipientId, encryptedMessage);

            // Send the message to the recipient
            await Clients.User(recipientId.ToString()).SendAsync("ReceiveMessage", senderId, encryptedMessage);
        }

        public async Task SendMessageGlobal(string encryptedMessage)
        {
            var senderIdString = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(senderIdString, out Guid senderId))
            {
                throw new HubException("Invalid sender ID.");
            }
            // Send the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", senderId.ToString(), encryptedMessage);
        }

        public async Task SendEncryptedMessage(string recipientId, object encryptedMessage)
        {
            var senderIdString = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(senderIdString, out Guid senderId))
            {
                throw new HubException("Invalid sender ID.");
            }
            await Clients.User(recipientId).SendAsync("ReceiveMessage", senderId.ToString(), encryptedMessage);
        }
    }
}