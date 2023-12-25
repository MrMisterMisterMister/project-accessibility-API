using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Company : User
    {
        [Required]
        public string Kvk { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Adres { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
        [Required]
        public string Url { get; set; } = null!;
        [Required]
        public string Contact { get; set; } = null!;
    }
}