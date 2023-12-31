using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Persistence;
using API.Services;
using API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    public class SignupController : BaseApiController
    {
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
                return BadRequest("Email is already taken");
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

    //     [HttpPost("panelmember")]
    //     public async Task<IActionResult> PanelmemberSignup([FromBody] PanelMember panelMember)
    //     {

    //         // check if the email already exists
    //         if (_dataContext.PanelMembers.Any(x => x.Email == panelMember.Email))
    //             // error
    //             return BadRequest(new
    //             {
    //                 message = "Email is already in use."
    //             });

    //         // Password validation
    //         // something something check for password length
    //         // also check if it contains 1 uppercase, 1 lowercase, 1 special character and 1 digit
    //         // for now just skipping it


    //         // hash password
    //         var passwordHash = new PasswordHasher<PanelMember>().HashPassword(panelMember, panelMember.Password);


    //         PanelMember newDisabled = new()
    //         {
    //             Email = panelMember.Email,
    //             Password = passwordHash,
    //             FirstName = panelMember.FirstName,
    //             LastName = panelMember.LastName
    //         };

    //         // save to database
    //         try
    //         {
    //             _dataContext.PanelMembers.Add(newDisabled);
    //             await _dataContext.SaveChangesAsync();

    //             return Ok("created new disabled");
    //         }
    //         catch (Exception e)
    //         {
    //             // some basic error handling
    //             Console.WriteLine(e.Message);
    //             return StatusCode(500);
    //         }
    //     }

    //     [HttpPost("company")]
    //     public async Task<IActionResult> CompanySignup([FromBody] Company company)
    //     {
    //         // check if the kvk already exists
    //         if (_dataContext.Companies.Any(x => x.Kvk == company.Kvk))
    //             // error
    //             return BadRequest(new
    //             {
    //                 message = "Kvk is already in use."
    //             });

    //         // Password validation
    //         // something something check for password length
    //         // also check if it contains 1 uppercase, 1 lowercase, 1 special character and 1 digit
    //         // for now just skipping it


    //         // hash password
    //         var passwordHash = new PasswordHasher<Company>().HashPassword(company, company.Password);


    //         Company newDisabled = new()
    //         {
    //             Email = company.Email,
    //             Password = passwordHash,
    //             Kvk = company.Kvk,
    //             Name = company.Name
    //         };

    //         // save to database
    //         try
    //         {
    //             _dataContext.Companies.Add(newDisabled);
    //             await _dataContext.SaveChangesAsync();

    //             return Ok("company created");
    //         }
    //         catch (Exception e)
    //         {
    //             // some basic error handling
    //             Console.WriteLine(e.Message);
    //             return StatusCode(500);
    //         }
    //     }
    }
}