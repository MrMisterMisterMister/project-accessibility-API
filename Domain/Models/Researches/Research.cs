using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Research
    {
        [Key]
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
        [Required]
        public Company Organizer { get; set; } = null!;
        public List<Participant> Participants { get; set; } = new List<Participant>();
    }
}