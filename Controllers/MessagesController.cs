using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatApp.Services;
using ChatApp.Models;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("chatroom/{chatRoomId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesByChatRoom(int chatRoomId, [FromQuery] int count = 50, [FromQuery] int skip = 0)
        {
            var messages = await _messageService.GetMessagesByChatRoomAsync(chatRoomId, count, skip);
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult<Message>> CreateMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMessage = await _messageService.CreateMessageAsync(message);
            return CreatedAtAction(nameof(GetMessage), new { id = createdMessage.Id }, createdMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, [FromBody] string content)
        {
            var result = await _messageService.UpdateMessageAsync(id, content);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { Message = "Message updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var result = await _messageService.DeleteMessageAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { Message = "Message deleted successfully" });
        }

        [HttpGet("private/{senderId}/{recipientId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetPrivateMessages(int senderId, int recipientId, [FromQuery] int count = 50)
        {
            var messages = await _messageService.GetPrivateMessagesAsync(senderId, recipientId, count);
            return Ok(messages);
        }
    }
}