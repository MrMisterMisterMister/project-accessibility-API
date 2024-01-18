using Application.Core;
using MediatR;
using Persistence;

namespace Application.DisabilityHandlers
{
    // Class responsible for handling the command to delete a disability
    public class DeleteDisability
    {
        // Command class represents the request to delete a disability
        public class Command : IRequest<Result<Unit>>
        {
            // Property to hold the ID of the disability to be deleted
            public int DisabilityId { get; set; }
        }

        // Handler class processes the DeleteDisability command
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            // Constructor initializes the handler with the required dependency
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            // Handle method executes the logic to delete a disability
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve the disability from the database based on its ID
                var disability = await _dataContext.Disabilities.FindAsync(request.DisabilityId);

                // If the disability is not found, return a failure result with a message
                if (disability == null) 
                    return Result<Unit>.Failure("DisabilityNotFound", "The disability could not be found.");

                // Remove the disability from the data context
                _dataContext.Remove(disability);

                // Save changes to the database and check if the deletion was successful
                var result = await _dataContext.SaveChangesAsync() > 0;

                // If the deletion was not successful, return a failure result with a message
                if (!result) 
                    return Result<Unit>.Failure("DisabilityFailedDelete", "The disability could not be deleted.");

                // Return a success result indicating that the disability was successfully deleted
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
