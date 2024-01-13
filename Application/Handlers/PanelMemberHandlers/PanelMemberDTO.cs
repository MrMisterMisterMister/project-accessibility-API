using System.ComponentModel.DataAnnotations;

namespace Application.Handlers.PanelMemberHandlers
{
    public class PanelMemberDTO
    {
        public string Id { get; set; } = null!;
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
        public ICollection<int> ParticipationsId { get; set; } = new List<int>();
    }
}