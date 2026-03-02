using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public bool IsGroupChat { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<User> Users { get; set; } = new List<User>();
        
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}