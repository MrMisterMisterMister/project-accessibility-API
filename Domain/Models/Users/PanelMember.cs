using System.ComponentModel.DataAnnotations;
using Domain.Models.Disabilities;

namespace Domain
{
    public class PanelMember : User
    {
        public int Guardian { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<ExpertDisability> Disabilities { get; set; } = new List<ExpertDisability>();
        public ICollection<ResearchParticipant> Participations { get; set; } = new List<ResearchParticipant>();
    }
}