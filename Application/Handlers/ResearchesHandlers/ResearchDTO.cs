using System.ComponentModel.DataAnnotations;
using Application.Handlers.PanelMemberHandlers;

namespace Application.Handlers.ResearchesHandlers
{
    public class ResearchDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Type { get; set; } = null!; // crack but lazy
        [Required]
        public string Category { get; set; } = null!; // crack but lazy
        [Required]
        public double Reward { get; set; }
        public string? OrganizerName { get; set; }
        [Required]
        public string OrganizerId { get; set; } = null!;
        public ICollection<PanelMemberDTO> Participants { get; set; } = new List<PanelMemberDTO>();
    }
}