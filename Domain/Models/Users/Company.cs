using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Company : User
    {
        [Required]
        public string Kvk { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        public string? Adres { get; set; }
        public string? Location { get; set; } // NOTICE this property can have multiple locations so need to be checked
        public string? Country { get; set; } // DITTO
        public string? Url { get; set; } // DITTO
        public string? Contact { get; set; } // DITTO
    }
}