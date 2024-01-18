using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ParticipantsHandlers
{
    // Handler for a participant to join a research
    public class AddResearchParticipant
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ResearchId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, ILogger<Handler> logger, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation(
                    $"Attempting to enroll participant in research with id '{request.ResearchId}");

                // eager load participants
                var research = await _dataContext.Researches
                    .Include(r => r.Participants)
                    .ThenInclude(p => p.PanelMember)
                    // We need to include the panel members to access participant information
                    // when checking if a participant is already enrolled in the research
                    .FirstOrDefaultAsync(r => r.Id == request.ResearchId);

                // Cancellation tokens are not currently used intentionally
                // There are no plans to implement them at this time

                if (research == null)
                    return Result<Unit>.Failure("ResearchNotFound", "The research could not be found.");

                _logger.LogInformation($"Research with research Id '{request.ResearchId}' found!");

                // getting the panelmember using email claims in the jwt token
                var participant = await _dataContext.PanelMembers.FirstOrDefaultAsync(x =>
                        x.Id == _userAccessor.GetId());

                if (participant == null)
                    return Result<Unit>.Failure("ParticipantNotFound", "The participant could not be found.");

                _logger.LogInformation($"Participant with Id '{participant.Id}' found!");

                // Checking synchronously since we already have the research
                // and participants loaded into memory
                var IsParticipant = research.Participants.Any(p => p.PanelMember == participant);

                if (IsParticipant)
                    return Result<Unit>.Failure("ParticipantAlreadyEnrolled", "The participant has already joined this research.");

                research.Participants.Add(new ResearchParticipant
                {
                    PanelMember = participant,
                    Research = research,
                    DateJoined = DateTime.UtcNow
                });

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("defaultMessage");

                _logger.LogInformation(
                    $"Participant {participant.UserName} successfully enrolled in research '{research.Title}'.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}