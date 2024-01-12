using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public RefreshToken? RefreshToken { get; set; }
    }
}