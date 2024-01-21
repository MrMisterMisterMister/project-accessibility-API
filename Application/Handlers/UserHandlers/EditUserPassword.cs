using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.UserHandlers
{
    public class EditUserPassword
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string? CurrentPassword { get; set; }
            public string? NewPassword { get; set; }
            public string? ConfirmPassword { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<User> _userManager;
            public Handler(UserManager<User> userManager, IUserAccessor userAccessor)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(request.NewPassword))
                    return Result<Unit>.Failure("PasswordIsNullOrEmpty", "Password cannot be null or empty.");

                if (request.NewPassword != request.ConfirmPassword)
                    return Result<Unit>.Failure("PasswordsDoNotMatch", "Passwords do not match.");

                if (string.IsNullOrEmpty(request.CurrentPassword))
                    return Result<Unit>.Failure("PasswordIsNullOrEmpty", "Password cannot be null or empty.");

                var user = await _userManager.FindByIdAsync(_userAccessor.GetId());

                if (user == null) return Result<Unit>.Failure("UserNotFound", "User could not be found.");

                // var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

                // whack versions since the above requires email confirmation

                var passwordCheck = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

                if (!passwordCheck)
                    return Result<Unit>.Failure("IncorrectPassword",
                        "Incorrect password. Please check your password and try again.");

                if (request.CurrentPassword == request.NewPassword)
                    return Result<Unit>.Failure("PasswordIsTheSame",
                        "New password cannot be the same as your old one.");

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("PasswordFailedUpdate", "User's password could not be updated.");
            }
        }
    }
}