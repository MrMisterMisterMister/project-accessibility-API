﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.ChatModels
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public required string SenderId { get; set; }
        public virtual User? Sender { get; set; }

        [Required]
        public string Content { get; set; } = null!;
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; }

        [ForeignKey("ChatId")]
        public int ChatId { get; set; }
        public virtual Chat? Chat { get; set; }
    }
}
