using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Research
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!; // crack but lazy
        public string Category { get; set; } = null!; // crack but lazy
        public double Reward { get; set; }
        public Company? Organizer { get; set; }
        public string? OrganizerId { get; set; }
        public ICollection<ResearchParticipant> Participants { get; set; } = new List<ResearchParticipant>();
    }
}