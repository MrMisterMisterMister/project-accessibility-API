using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Chat{
    
public class Chat
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string User1Id { get; set; } = null!; // ID of the first User
    public virtual User User1 { get; set; } // Navigation property for the first User

    [Required]
    public string User2Id { get; set; } = null!; // ID of the second User
    public virtual User User2 { get; set; } // Navigation property for the second User

    [InverseProperty("Chat")]
    public List<Message>? Messages { get; set; }
}