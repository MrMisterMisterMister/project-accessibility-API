using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Chat{
    public class Chat{
        [Key]
        public int id {get;set;}
        [Required]
        public string Title {get;set;} = null!;
        [InverseProperty("Chat")]
        public List<Message>?Messages {get;set;}
    }
}