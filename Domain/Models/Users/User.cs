using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        // Using a list allowes a user to have concurrent sessions
        // across different devices, this will however make it more complex
        // and increase storage use
        // TODO when a user deletes their account, tokens need to be deleted as well
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}