using Application.Core;
using Application.Interfaces;
using Domain.Models.Disabilities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.ExpertDisabilityHandlers
{
    // Class responsible for handling the command to add a disability to an expert
    public class AddExpertDisability
    {
        // Command class represents the request to add a disability to an expert
        public class Command : IRequest<Result<Unit>>
        {
            // Property to hold the ID of the disability to be added
            public int DisabilityId { get; set; }
        }

        // Handler class processes the AddExpertDisability command
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            // Constructor initializes the handler with required dependencies
            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _dataContext = dataContext;
            }

            // Handle method executes the logic to add a disability to an expert
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve the disability from the database based on its ID
                var disability = await _dataContext.Disabilities
                    .Include(x => x.Experts)
                    .ThenInclude(x => x.PanelMember)
                    .FirstOrDefaultAsync(x => x.Id == request.DisabilityId);

                // If the disability is not found, return a failure result with a message
                if (disability == null)
                    return Result<Unit>.Failure("DisabilityNotFound", "The disability could not be found.");

                // Retrieve the expert (panel member) from the database based on their email
                var expert = await _dataContext.PanelMembers.FirstOrDefaultAsync(x =>
                    x.Email == _userAccessor.GetEmail()
                );

                // If the expert is not found, return a failure result with a message
                if (expert == null)
                    return Result<Unit>.Failure("PanelMemberNotFound", "The panel member could not be found.");

                // Check if the expert already has the specified disability
                var hasDisability = disability.Experts.FirstOrDefault(x => x.PanelMember == expert);

                // If the expert already has the disability, return a failure result with a message
                if (hasDisability != null)
                    return Result<Unit>.Failure("ExpertHasDisability", "The expert already has this disability.");

                // Add the disability to the expert's list of disabilities
                disability.Experts.Add(new ExpertDisability
                {
                    Disability = disability,
                    PanelMember = expert
                });

                // Save changes to the database and check if the update was successful
                var result = await _dataContext.SaveChangesAsync() > 0;

                // If the update was not successful, return a failure result with a default message
                if (!result)
                    return Result<Unit>.Failure("defaultMessage");

                // Return a success result indicating that the disability was successfully added to the expert
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
