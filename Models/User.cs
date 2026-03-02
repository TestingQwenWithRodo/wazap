using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public bool IsOnline { get; set; } = false;
        
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;
        
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}