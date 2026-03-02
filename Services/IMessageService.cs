using ChatApp.Models;

namespace ChatApp.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Message>> GetMessagesByChatRoomAsync(int chatRoomId, int count = 50, int skip = 0);
        Task<Message> GetMessageByIdAsync(int id);
        Task<Message> CreateMessageAsync(Message message);
        Task<bool> UpdateMessageAsync(int messageId, string newContent);
        Task<bool> DeleteMessageAsync(int messageId);
        Task<IEnumerable<Message>> GetPrivateMessagesAsync(int senderId, int recipientId, int count = 50);
    }
}