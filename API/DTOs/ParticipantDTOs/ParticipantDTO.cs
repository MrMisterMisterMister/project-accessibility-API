using System.ComponentModel.DataAnnotations;
using Domain;

namespace API.DTOs
{
    public class ParticipantDTO
    {
        public int ResearchId { get; set; } 
        public string PanelMemberId { get; set; } = null!;
        public DateTime DateJoined { get; set; }
        public string? Status { get; set; }
    }
}