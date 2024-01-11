using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RegisterDTOs
{
    public class RegisterPanelMemberDTO : RegisterDTO
    {
        public int Guardian { get; set; } // TODO ...
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}