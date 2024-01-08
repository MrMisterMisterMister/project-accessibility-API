using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public async Task SeedRoles()
        {
            foreach (var role in Enum.GetNames(typeof(RoleTypes)))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // beetje whack, will use in controller soon... maybe
        public async Task<Result<Unit>> AssignRoleToUser(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<Unit>.Failure("User not found");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
                return Result<Unit>.Failure("Role not found");

            var userAlreadyHasRole = await _userManager.IsInRoleAsync(user, roleName);

            if (userAlreadyHasRole)
                return Result<Unit>.Failure("User already has this role");

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
                return Result<Unit>.Failure($"Failed to assign role to {user}");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}