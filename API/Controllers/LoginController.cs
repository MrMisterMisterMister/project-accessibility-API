using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using Persistence;

namespace API.Controllers
{
    public class LoginController : BaseApiController
    {
        private readonly DatabaseContext _databaseContext;

        public LoginController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        // also cracky code
        // will fix later 
        // maybe
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            // check if the user exists by looking up the email
            var userInDb = _databaseContext.Users.FirstOrDefault(x => x.Email == user.Email);

            if (userInDb != null)
            {
                // some magic to compare the hashed password and that what was inputted
                var passwordVerificationResult = new PasswordHasher<User>().VerifyHashedPassword(user, userInDb.Password, user.Password);

                // now check if password is correct
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    // need to implement token still
                    // also need to make sure the Remember Me option works
                    // so like generate a Cookie that saves the login
                    // that expires after x days or something so they dont have to login everytime

                    Console.WriteLine("if you see this, you probably are logged in");

                    // approve it
                    return Ok(new { message = "大" });
                }

                return BadRequest(new { message = "password incorrect" });
            }

            return BadRequest(new { message = "your mom" });
        }
    }
}