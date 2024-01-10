using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }
        public Research Research { get; set; } = null!;
        public PanelMember PanelMember { get; set; } = null!;
        public string? Status { get; set; }
    }
}
