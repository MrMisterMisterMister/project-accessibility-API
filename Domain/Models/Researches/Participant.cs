using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Participant
    {
        [Key]
        [Column(Order = 0)]
        [Required]
        [ForeignKey("Research")]
        public int ResearchId { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        [ForeignKey("PanelMember")]
        public Guid PanelMemberId { get; set;}

        public string ?Status { get; set; }

        public Research Research { get; set; } = null!;
        public PanelMember PanelMember { get; set; } = null!;
    }
}
