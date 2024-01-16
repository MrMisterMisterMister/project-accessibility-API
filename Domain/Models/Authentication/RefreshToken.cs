using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class RefreshToken
    {
        public int Id { get; set; }
        [Required]
        public User User { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
        public DateTime? Revoked { get; set; } // Timestamp when revoked
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => Revoked == null && !IsExpired;
    }
}