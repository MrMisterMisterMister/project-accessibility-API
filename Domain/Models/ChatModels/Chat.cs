using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.ChatModels{
    public class Chat{
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        [Required]
        public string? User1Id { get; set; }
        public string? User1Email { get; set; }
        public virtual User? User1 { get; set; }

        [Required]
        public string? User2Id { get; set; }
        public string? User2Email { get; set; }
        public virtual User? User2 { get; set; }

        public virtual ICollection<Message>? Messages { get; set; }
    }
}