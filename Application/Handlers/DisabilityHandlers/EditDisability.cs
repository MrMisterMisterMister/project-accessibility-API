using Application.Core;
using MediatR;
using AutoMapper;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Models.Disabilities;

namespace Application.DisabilityHandlers
{
    // Class responsible for handling the command to edit a disability
    public class EditDisability
    {
        // Command class represents the request to edit a disability
        public class Command : IRequest<Result<Unit>>
        {
            // Property to hold the details of the disability to be edited
            public Disability Disability { get; set; } = null!;
        }

        // Handler class processes the EditDisability command
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            // Constructor initializes the handler with required dependencies
            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            // Handle method executes the logic to edit a disability
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve the disability from the database based on its ID
                // using eagerloading
                var disability = await _dataContext.Disabilities
                    .Include(x => x.Experts)
                    .ThenInclude(x => x.PanelMember)
                    .FirstOrDefaultAsync(x => x.Id == request.Disability.Id);

                // If the disability is not found, return a failure result with a message
                if (disability == null)
                    return Result<Unit>.Failure("DisabilityNotFound", "The disability could not be found.");

                // Copy the existing experts associated with the disability to the request disability
                foreach (var expert in disability.Experts)
                    request.Disability.Experts.Add(expert);

                // Map the properties of the request disability to the existing disability using AutoMapper
                _mapper.Map(request.Disability, disability);

                // Save changes to the database and check if the update was successful
                var result = await _dataContext.SaveChangesAsync() > 0;

                // If the update was not successful, return a failure result with a message
                if (!result)
                    return Result<Unit>.Failure("DisabilityFailedUpdate", "The disability could not be updated.");

                // Return a success result indicating that the disability was successfully updated
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
