using ChatApp.Data;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesByChatRoomAsync(int chatRoomId, int count = 50, int skip = 0)
        {
            return await _context.Messages
                .Where(m => m.ChatRoomId == chatRoomId)
                .OrderByDescending(m => m.Timestamp)
                .Skip(skip)
                .Take(count)
                .Include(m => m.User)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _context.Messages
                .Include(m => m.User)
                .Include(m => m.ChatRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> UpdateMessageAsync(int messageId, string newContent)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                message.Content = newContent;
                message.IsEdited = true;
                message.EditedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Message>> GetPrivateMessagesAsync(int senderId, int recipientId, int count = 50)
        {
            return await _context.Messages
                .Where(m => (m.UserId == senderId && m.ChatRoomId == null) || 
                           (m.UserId == recipientId && m.ChatRoomId == null))
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .Include(m => m.User)
                .ToListAsync();
        }
    }
}