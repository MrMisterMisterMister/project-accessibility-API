using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }
        public Research Research { get; set; } = null!;
        public PanelMember PanelMember { get; set; } = null!;
        public DateTime DateJoined { get; set; }
        public string? Status { get; set; }
    }
}
