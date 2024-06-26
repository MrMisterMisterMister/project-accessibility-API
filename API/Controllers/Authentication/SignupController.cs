using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using Microsoft.AspNetCore.Authorization;
using API.Services;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.DTOs.RegisterDTOs;

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
                // Update the user with refresh token
                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                return new UserDTO { Token = await _tokenService.CreateAndSetCookie(user, roles.ToList()) };
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
                DateOfBirth = DateTime.TryParse(registerPanelMemberDTO.DateOfBirth, out DateTime parsedDate) ? parsedDate : DateTime.MinValue,
                Address = registerPanelMemberDTO.Address,
                PostalCode = registerPanelMemberDTO.PostalCode,
                City = registerPanelMemberDTO.City,
                Country = registerPanelMemberDTO.Country
            };

            var result = await _userManager.CreateAsync(panelMember, registerPanelMemberDTO.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(panelMember, nameof(RoleTypes.PanelMember));

                await _userManager.UpdateAsync(panelMember);

                var roles = await _userManager.GetRolesAsync(panelMember);
                return new UserDTO { Token = await _tokenService.CreateAndSetCookie(panelMember, roles.ToList()) };
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
                CompanyName = registerCompanyDTO.CompanyName,
                Phone = registerCompanyDTO.Phone,
                Address = registerCompanyDTO.Address,
                PostalCode = registerCompanyDTO.PostalCode,
                Province = registerCompanyDTO.Province,
                Country = registerCompanyDTO.Country,
                WebsiteUrl = registerCompanyDTO.WebsiteUrl,
                ContactPerson = registerCompanyDTO.ContactPerson
            };

            var result = await _userManager.CreateAsync(company, registerCompanyDTO.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(company, nameof(RoleTypes.Company));

                await _userManager.UpdateAsync(company);

                var roles = await _userManager.GetRolesAsync(company);
                return new UserDTO { Token = await _tokenService.CreateAndSetCookie(company, roles.ToList()) };
            }

            return BadRequest(result.Errors);
        }
    }
}