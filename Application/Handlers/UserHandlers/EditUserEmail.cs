using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.UserHandlers
{
    public class EditUserEmail
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string? NewEmail { get; set; }
            public string? ConfirmEmail { get; set; }
            public string? Password { get; set; }
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
                if (string.IsNullOrEmpty(request.NewEmail))
                    return Result<Unit>.Failure("EmailIsNullOrEmpty", "Email cannot be null or empty.");

                if (request.NewEmail != request.ConfirmEmail)
                    return Result<Unit>.Failure("EmailsDoNotMatch", "Emails do not match.");

                if (string.IsNullOrEmpty(request.Password))
                    return Result<Unit>.Failure("PasswordIsNullOrEmpty", "Password cannot be null or empty.");

                var user = await _userManager.FindByIdAsync(_userAccessor.GetId());

                if (user == null) return Result<Unit>.Failure("UserNotFound", "User could not be found.");

                if (user.Email == request.NewEmail)
                    return Result<Unit>.Failure("EmailIsTheSame", "Email cannot be the same.");

                var emailCheck = await _userManager.FindByEmailAsync(request.NewEmail);

                if (emailCheck != null)
                    return Result<Unit>.Failure("EmailIsTaken", "Email is in use by someone else");

                var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);

                if (!passwordCheck)
                    return Result<Unit>.Failure("IncorrectPassword",
                        "Incorrect password. Please check your password and try again.");

                // var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email);

                // var result = await _userManager.ChangeEmailAsync(user, request.Email, token);

                // whack versions since the above requires email confirmation

                var normalizedEmail = _userManager.NormalizeEmail(request.NewEmail);

                user.Email = request.NewEmail;
                user.NormalizedEmail = normalizedEmail;
                user.UserName = request.NewEmail;
                user.NormalizedUserName = normalizedEmail;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return Result<Unit>.Success(Unit.Value);

                return Result<Unit>.Failure("EmailFailedUpdate", "User's email could not be updated.");
            }
        }
    }
}