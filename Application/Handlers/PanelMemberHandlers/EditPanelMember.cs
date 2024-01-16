using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    // Command class represents the request to edit a panel member
    public class EditPanelMember
    {
        // Command class inherits IRequest<Result<Unit>>, indicating the expected result of the operation
        public class Command : IRequest<Result<Unit>>
        {
            // PanelMember property holds the data for the panel member to be edited
            public PanelMember PanelMember { get; set; } = null!;
        }

        // Handler class processes the EditPanelMember command
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            // Constructor initializes the handler with required dependencies
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            // Handle method executes the logic to edit the panel member
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // Retrieve the existing panel member from the database based on its ID
                var panelMember = await _dataContext.PanelMembers.FindAsync(request.PanelMember.Id);

                // If the panel member is not found, return a failure result with a message
                if (panelMember == null) 
                    return Result<Unit>.Failure("PanelMemberNotFound", "Panel member could not be found.");

                // Copy some essential properties from the existing panel member to the request panel member
                // This is done to prevent unintended changes to certain properties
                request.PanelMember.UserName = panelMember.UserName;
                request.PanelMember.NormalizedUserName = panelMember.NormalizedUserName;
                request.PanelMember.Email = panelMember.Email;
                request.PanelMember.NormalizedEmail = panelMember.NormalizedEmail;
                request.PanelMember.PasswordHash = panelMember.PasswordHash;

                // Copy the existing participations and disabilities to the request panel member
                foreach (var research in panelMember.Participations)
                    request.PanelMember.Participations.Add(research);

                foreach (var disability in panelMember.Disabilities)
                    request.PanelMember.Disabilities.Add(disability);

                // Map the properties of the request panel member to the existing panel member using AutoMapper
                _mapper.Map(request.PanelMember, panelMember);

                // Save changes to the database and check if the update was successful
                var result = await _dataContext.SaveChangesAsync() > 0;

                // If the update was not successful, return a failure result with a message
                if (!result) 
                    return Result<Unit>.Failure("PanelMemberFailedUpdate", "Failed to update panel member.");

                // Return a success result indicating that the panel member was successfully updated
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
