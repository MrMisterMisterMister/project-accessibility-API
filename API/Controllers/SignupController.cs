using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using Persistence;

namespace API.Controllers
{
    public class SignupController : BaseApiController
    {
        private readonly DatabaseContext _databaseContext;

        public SignupController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        // crackiest code ever
        // will improve it later
        [HttpPost("panelmember")]
        public async Task<IActionResult> PanelmemberSignup([FromBody] PanelMember panelMember)
        {

            // check if the email already exists
            if (_databaseContext.PanelMembers.Any(x => x.Email == panelMember.Email))
                // error
                return BadRequest(new { 
                    message = "Email is already in use." 
                });

            // Password validation
            // something something check for password length
            // also check if it contains 1 uppercase, 1 lowercase, 1 special character and 1 digit
            // for now just skipping it


            // hash password
            var passwordHash = new PasswordHasher<PanelMember>().HashPassword(panelMember, panelMember.Password);


            PanelMember newDisabled = new()
            {
                Email = panelMember.Email,
                Password = passwordHash,
                FirstName = panelMember.FirstName,
                LastName = panelMember.LastName
            };

            // save to database
            try
            {
                _databaseContext.PanelMembers.Add(newDisabled);
                await _databaseContext.SaveChangesAsync();

                return Ok("created new disabled");
            }
            catch (Exception e)
            {
                // some basic error handling
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("company")]
        public async Task<IActionResult> CompanySignup([FromBody] Company company)
        {
            // check if the kvk already exists
            if (_databaseContext.Companies.Any(x => x.Kvk == company.Kvk))
                // error
                return BadRequest(new { 
                    message = "Kvk is already in use." 
                });

            // Password validation
            // something something check for password length
            // also check if it contains 1 uppercase, 1 lowercase, 1 special character and 1 digit
            // for now just skipping it


            // hash password
            var passwordHash = new PasswordHasher<Company>().HashPassword(company, company.Password);


            Company newDisabled = new()
            {
                Email = company.Email,
                Password = passwordHash,
                Kvk = company.Kvk,
                Name = company.Name
            };

            // save to database
            try
            {
                _databaseContext.Companies.Add(newDisabled);
                await _databaseContext.SaveChangesAsync();

                return Ok("company created");
            }
            catch (Exception e)
            {
                // some basic error handling
                Console.WriteLine(e.Message);
                return StatusCode(500);
            }
        }
    }
}