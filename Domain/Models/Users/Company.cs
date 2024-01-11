using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Company : User
    {
        [Required]
        public string Kvk { get; set; } = null!;
        [Required]
        public string CompanyName { get; set; } = null!;
        [Phone]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; } // NOTICE this property can have multiple locations so need to be checked
        public string? Province { get; set; } // NOTICE this property can have multiple locations so need to be checked
        public string? Country { get; set; } // DITTO
        public string? WebsiteUrl { get; set; } // DITTO
        public string? ContactPerson { get; set; } // DITTO
    }
}