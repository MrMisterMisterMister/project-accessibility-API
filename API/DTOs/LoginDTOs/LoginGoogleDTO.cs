using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class LoginGoogleDTO
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        [JsonPropertyName("given_name")]
        public string FirstName { get; set; } = null!;
        [Required]
        [JsonPropertyName("family_name")]
        public string LastName { get; set; } = null!;
    }
}