using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int? ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
        
        public bool IsEdited { get; set; } = false;
        
        public DateTime? EditedAt { get; set; }
    }
}