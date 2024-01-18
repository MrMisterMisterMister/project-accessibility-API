using Application.Core;
using Domain.Models.Disabilities;
using MediatR;
using Persistence;

namespace Application.DisabilityHandlers
{
    // Class responsible for handling the command to create a new disability
    public class CreateDisability
    {
        // Command class represents the request to create a new disability
        public class Command : IRequest<Result<Unit>>
        {
            // Property to hold the details of the disability to be created
            public Disability Disability { get; set; } = null!;
        }

        // Handler class processes the CreateDisability command
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            // Constructor initializes the handler with the required dependency
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            // Handle method executes the logic to create a new disability
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Add the new disability to the data context
                _dataContext.Add(request.Disability);

                // Save changes to the database and check if the creation was successful
                var result = await _dataContext.SaveChangesAsync() > 0;

                // If the creation was not successful, return a failure result with a message
                if (!result) 
                    return Result<Unit>.Failure("DisabilityFailedCreate", "Failed to create disability.");

                // Return a success result indicating that the disability was successfully created
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
