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
        [Required]
        public string Zipcode { get; set; } = null!;
        [Required]
        public DateTime DateOfBirth { get; set; }
        
    }
}