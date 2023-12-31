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

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDTO.Email))
            {
                return BadRequest(
                    new
                    {
                        code = "EmailTaken",
                        description = "This email address is already taken."
                    }
                );
            }

            var user = new User
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded) return new UserDTO { Token = _tokenService.CreateToken(user) };

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

            if (result.Succeeded) return new UserDTO { Token = _tokenService.CreateToken(panelMember) };

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

            if (result.Succeeded) return new UserDTO { Token = _tokenService.CreateToken(company) };

            return BadRequest(result.Errors);
        }
    }
}