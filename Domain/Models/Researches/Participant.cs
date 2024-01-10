using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Participant
    {
        [Key]
        [ForeignKey("Research")]
        [Required]
        public int ResearchId { get; set; }

        [Key]
        [ForeignKey("PanelMember")]
         [Required]
        public Guid PanelMemberId { get; set;}

        public string ?Status { get; set; }

        public Research Research { get; set; } = null!;
        public PanelMember PanelMember { get; set; } = null!;
    }
}
