using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RegisterDTOs
{
    public class RegisterCompanyDTO : RegisterDTO
    {
        [Required]
        public string Kvk { get; set; } = null!;
        [Required]
        public string CompanyName { get; set; } = null!;
        [Phone]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? ContactPerson { get; set; }
    }
}