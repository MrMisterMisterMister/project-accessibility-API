using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Guardian
    {
        [Required]
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}