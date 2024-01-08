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
        public string? Zipcode { get; set; }
        public string? DateOfBirth { get; set; }
    }
}