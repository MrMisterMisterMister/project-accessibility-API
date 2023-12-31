using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}