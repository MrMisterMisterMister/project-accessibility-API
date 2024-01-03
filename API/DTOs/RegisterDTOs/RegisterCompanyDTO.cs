using System.ComponentModel.DataAnnotations;

namespace API.DTOs.RegisterDTOs
{
    public class RegisterCompanyDTO : RegisterDTO
    {
        [Required]
        public string Kvk { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        public string? Adres { get; set; }
        public string? Location { get; set; }
        public string? Country { get; set; }
        public string? Url { get; set; }
        public string? Contact { get; set; }
    }
}