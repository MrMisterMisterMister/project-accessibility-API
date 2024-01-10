using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }

        public int ResearchId { get; set; }

        public Guid PanelMemberId { get; set; }

        public string? Status { get; set; }
    }
}
