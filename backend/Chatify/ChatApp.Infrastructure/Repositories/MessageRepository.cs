using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Core.Models;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddMessageAsync(Message message)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(Guid userId, Guid contactId)
        {
            return await _dbContext.Messages
                .AsNoTracking()
                .Where(m =>
                    (m.SenderId == userId && m.RecipientId == contactId) ||
                    (m.SenderId == contactId && m.RecipientId == userId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetRecentConversationsAsync(Guid userId)
        {
            var messages = await _dbContext.Messages
                .AsNoTracking()
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderByDescending(m => m.Timestamp)
                .ToListAsync();

            return messages;
        }
    }
}
