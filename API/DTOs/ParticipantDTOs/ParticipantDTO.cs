using System.ComponentModel.DataAnnotations;
using Domain;

namespace API.DTOs
{
    public class ParticipantDTO
    {
        public Research Research { get; set; } = null!;
        public PanelMember PanelMember { get; set; } = null!;
        public DateTime DateJoined { get; set; }
        public string? Status { get; set; }
    }
}