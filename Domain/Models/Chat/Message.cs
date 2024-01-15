using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Chat
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string SenderId { get; set; } // ID of the User who sent the message
        public virtual User Sender { get; set; } // Navigation property

        [Required]
        public string ReceiverId { get; set; } // ID of the User who is to receive the message
        public virtual User Receiver { get; set; } // Navigation property

        [Required]
        public string Content { get; set; } // Content of the message

        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Time when the message was sent

        public bool IsRead { get; set; } = false; // Flag to indicate if the message has been read
    }
}
