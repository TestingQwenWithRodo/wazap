using Microsoft.AspNetCore.SignalR;
using ChatApp.Models;
using ChatApp.Services;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public ChatHub(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        public async Task SendMessage(int userId, int? chatRoomId, string content)
        {
            var message = new Message
            {
                UserId = userId,
                ChatRoomId = chatRoomId,
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            var createdMessage = await _messageService.CreateMessageAsync(message);

            if (chatRoomId.HasValue)
            {
                // Broadcast to all users in the chat room
                await Clients.Group($"chatroom-{chatRoomId}").SendAsync("ReceiveMessage", 
                    createdMessage.Id, 
                    createdMessage.User.Username, 
                    createdMessage.Content, 
                    createdMessage.Timestamp);
            }
            else
            {
                // For private messages, we would need to implement a way to identify the recipient
                // For now, just broadcast to sender as a placeholder
                await Clients.Caller.SendAsync("ReceiveMessage", 
                    createdMessage.Id, 
                    createdMessage.User.Username, 
                    createdMessage.Content, 
                    createdMessage.Timestamp);
            }
        }

        public async Task JoinChatRoom(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserJoined", $"{Context.UserIdentifier} joined {groupName}");
        }

        public async Task LeaveChatRoom(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("UserLeft", $"{Context.UserIdentifier} left {groupName}");
        }

        public async Task SendTypingIndicator(int chatRoomId, string username)
        {
            await Clients.OthersInGroup($"chatroom-{chatRoomId}").SendAsync("ReceiveTypingIndicator", username);
        }

        public override async Task OnConnectedAsync()
        {
            // Update user status to online when they connect
            if (int.TryParse(Context.UserIdentifier, out int userId))
            {
                await _userService.UpdateUserStatusAsync(userId, true);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Update user status to offline when they disconnect
            if (int.TryParse(Context.UserIdentifier, out int userId))
            {
                await _userService.UpdateUserStatusAsync(userId, false);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}