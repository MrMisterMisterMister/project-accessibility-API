using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class RefreshToken
    {
        [Key] 
        // this isn't actually needed since a property with Id and if there
        // is only one will autimatically be seen as the PK by EF Core
        public int Id { get; set; }
        [Required]
        public User User { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        public DateTime? Revoked { get; set; } // Timestamp when revoked
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool isExpired => DateTime.UtcNow >= Expires;
        public bool isActive => Revoked == null && !isExpired;
    }
}