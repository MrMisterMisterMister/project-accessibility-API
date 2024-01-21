using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.PanelMemberDisabilityHandlers
{
    // Class responsible for handling the command to delete a disability from an panelMember
    public class DeletePanelMemberDisability
    {
        // Command class represents the request to delete a disability from an panelMember
        public class Command : IRequest<Result<Unit>>
        {
            // Property to hold the ID of the disability to be deleted
            public int DisabilityId { get; set; }
        }

        // Handler class processes the DeletePanelMemberDisability command
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

            // Handle method executes the logic to delete a disability from an panelMember
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve the disability from the database based on its ID
                var disability = await _dataContext.Disabilities
                    .Include(x => x.PanelMembers)
                    .ThenInclude(x => x.PanelMember)
                    .FirstOrDefaultAsync(x => x.Id == request.DisabilityId);

                // If the disability is not found, return a failure result with a message
                if (disability == null)
                    return Result<Unit>.Failure("DisabilityNotFound", "The disability could not be found.");

                // Retrieve the panelMember (panel member) from the database based on their email
                var panelMember = await _dataContext.PanelMembers.FirstOrDefaultAsync(x =>
                    x.Email == _userAccessor.GetEmail()
                );

                // If the panelMember is not found, return a failure result with a message
                if (panelMember == null)
                    return Result<Unit>.Failure("PanelMemberNotFound", "The panel member could not be found.");

                // Check if the panelMember has the specified disability
                var hasDisability = disability.PanelMembers.FirstOrDefault(x => x.PanelMember == panelMember);

                // If the panelMember does not have the disability, return a failure result with a message
                if (hasDisability == null)
                    return Result<Unit>.Failure("PanelMemberDoesNotHaveDisability", "The panelMember does not have this disability.");

                // Remove the disability from the panelMember's list of disabilities
                disability.PanelMembers.Remove(hasDisability);

                // Save changes to the database and check if the update was successful
                var result = await _dataContext.SaveChangesAsync() > 0;

                // If the update was not successful, return a failure result with a default message
                if (!result) return Result<Unit>.Failure("defaultMessage");

                // Return a success result indicating that the disability was successfully deleted from the panelMember
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
