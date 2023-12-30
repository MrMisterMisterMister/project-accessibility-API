using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class PanelMember : User
    {
        public int Guardian { get; set; } // TODO ...
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string? Zipcode { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}