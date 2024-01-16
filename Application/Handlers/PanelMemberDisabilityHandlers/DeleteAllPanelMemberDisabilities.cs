using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.PanelMemberDisabilityHandlers
{
    public class DeleteAllPanelMemberDisabilities
    {
        public class Command : IRequest<Result<Unit>> { }

        // Handler class processes the DeleteAllPanelMemberDisability command
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

            // Handle method executes the logic to delete all disabilities from a panelMember
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve the panelMember (panel member) from the database based on their email
                var panelMember = await _dataContext.PanelMembers
                    .Include(x => x.Disabilities)
                    .ThenInclude(d => d.Disability)
                    .FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail()
                );

                // If the panelMember is not found, return a failure result with a message
                if (panelMember == null)
                    return Result<Unit>.Failure("PanelMemberNotFound", "The panel member could not be found.");

                // If there are no disabilities found, return success
                if (!panelMember.Disabilities.Any()) return Result<Unit>.Success(Unit.Value);

                // Remove the disabilities from the panelMember's list of disabilities
                panelMember.Disabilities.Clear();

                // Save changes to the database and check if the update was successful
                var result = await _dataContext.SaveChangesAsync() > 0;

                // If the update was not successful, return a failure result with a default message
                if (!result)
                    return Result<Unit>.Failure("defaultMessage");

                // Return a success result indicating that the disability was successfully deleted from the panelMember
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}