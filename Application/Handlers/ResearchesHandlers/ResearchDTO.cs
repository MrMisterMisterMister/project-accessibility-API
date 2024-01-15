using System.ComponentModel.DataAnnotations;
using Application.Handlers.PanelMemberHandlers;

namespace Application.Handlers.ResearchesHandlers
{
    public class ResearchDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!; // crack but lazy
        public string Category { get; set; } = null!; // crack but lazy
        public double Reward { get; set; }
        public string? OrganizerName { get; set; }
        public string? OrganizerId { get; set; } = null!;
        public ICollection<PanelMemberDTO> Participants { get; set; } = new List<PanelMemberDTO>();
    }
}