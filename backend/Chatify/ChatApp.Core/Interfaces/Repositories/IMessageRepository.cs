using ChatApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync(Guid userId, Guid contactId);
        Task<IEnumerable<Message>> GetRecentConversationsAsync(Guid userId);
    }
}
