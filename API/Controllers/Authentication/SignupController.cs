using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using Microsoft.AspNetCore.Authorization;
using API.Services;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.DTOs.RegisterDTOs;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [AllowAnonymous]
    public class SignupController : BaseApiController
    {
        // on crack
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public SignupController(UserManager<User> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("google-signup")]
        public async Task<ActionResult<UserDTO>> GoogleSignUp([FromBody] string googleJWTToken)
        {
            // Decode the JWT token to extract user information
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(googleJWTToken) as JwtSecurityToken;

            // Extract user email from the decoded JWT token
            var userEmail = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "email")?.Value;
            var userFirstName = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var userLastName = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "family_name")?.Value;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userFirstName) || string.IsNullOrEmpty(userLastName))
            {
                return BadRequest("Invalid Google JWT token or missing required information.");
            }

            // Check if the email already exists in the database
            var existingUser = await _userManager.FindByEmailAsync(userEmail);

            if (existingUser != null)
            {
                // If the user already exists, return an error indicating the email is taken
                return BadRequest(new
                {
                    code = "EmailTaken",
                    description = "This email address is already registered."
                });
            }

            // If the email doesn't exist, create a new panelmember in the database
            var panelMember = new PanelMember
            {
                Email = userEmail,
                FirstName = userFirstName,
                LastName = userLastName
            };

            var result = await _userManager.CreateAsync(panelMember);

            if (result.Succeeded)
            {
                // User creation successful, generate a JWT token for the new user
                var roles = await _userManager.GetRolesAsync(panelMember);
                return new UserDTO { Token = _tokenService.CreateAndSetCookie(panelMember, roles.ToList()) };
            }

            // Return errors if user creation fails
            return BadRequest(result.Errors);
        }

        // Handles user signup and generates a JWT token as a cookie
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Signup(RegisterDTO registerDTO)
        {
            // Check if the provided email already exists in the database
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDTO.Email))
            {
                return BadRequest(new
                {
                    code = "EmailTaken",
                    description = "This email address is already taken."
                });
            }

            // Create a new user instance with the provided email and password
            var user = new User
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            // Attempt to create the user in the database
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            // If the user creation is successful, return a JWT token as a cookie
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new UserDTO { Token = _tokenService.CreateAndSetCookie(user, roles.ToList()) };
            }

            // Return error messages if user creation fails
            return BadRequest(result.Errors);
        }

        [HttpPost("panelmember")]
        public async Task<ActionResult<UserDTO>> PanelmemberSignup(RegisterPanelMemberDTO registerPanelMemberDTO)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerPanelMemberDTO.Email))
            {
                return BadRequest(
                    new
                    {
                        code = "EmailTaken",
                        description = "This email address is already taken."
                    }
                );
            }

            var panelMember = new PanelMember
            {
                Email = registerPanelMemberDTO.Email,
                UserName = registerPanelMemberDTO.Email,
                Guardian = registerPanelMemberDTO.Guardian,
                FirstName = registerPanelMemberDTO.FirstName,
                LastName = registerPanelMemberDTO.LastName,
                Zipcode = registerPanelMemberDTO.Zipcode,
                DateOfBirth = DateTime.TryParse(registerPanelMemberDTO.DateOfBirth,
                    out DateTime parsedDate)
                    ? parsedDate : DateTime.MinValue
            };

            var result = await _userManager.CreateAsync(panelMember, registerPanelMemberDTO.Password);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(panelMember);
                return new UserDTO { Token = _tokenService.CreateAndSetCookie(panelMember, roles.ToList()) };
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("company")]
        public async Task<ActionResult<UserDTO>> CompanySignup(RegisterCompanyDTO registerCompanyDTO)
        {
            if (await _userManager.Users.OfType<Company>().AnyAsync(x => x.Kvk == registerCompanyDTO.Kvk))
            {
                return BadRequest(
                    new
                    {
                        code = "KvkExists",
                        description = "This Kvk number is already registered."
                    }
                );
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerCompanyDTO.Email))
            {
                return BadRequest(
                    new
                    {
                        code = "EmailTaken",
                        description = "This email address is already taken."
                    }
                );
            }

            var company = new Company
            {
                Email = registerCompanyDTO.Email,
                UserName = registerCompanyDTO.Email,
                Kvk = registerCompanyDTO.Kvk,
                Name = registerCompanyDTO.Name,
                Adres = registerCompanyDTO.Adres,
                Location = registerCompanyDTO.Location,
                Country = registerCompanyDTO.Country,
                Url = registerCompanyDTO.Url,
                Contact = registerCompanyDTO.Contact
            };

            var result = await _userManager.CreateAsync(company, registerCompanyDTO.Password);

            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(company);
                return new UserDTO { Token = _tokenService.CreateAndSetCookie(company, roles.ToList()) };
            }

            return BadRequest(result.Errors);
        }
    }
}