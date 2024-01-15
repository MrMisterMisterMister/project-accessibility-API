using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ParticipantsHandlers
{
    public class RemoveResearchParticipants
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
                _logger.LogInformation($"Fetching research with Id: {request.ResearchId}");

                // eager load participants
                var research = await _dataContext.Researches
                    .Include(r => r.Participants)
                    .ThenInclude(p => p.PanelMember)
                    .FirstOrDefaultAsync(r => r.Id == request.ResearchId);

                if (research == null)
                    return Result<Unit>.Failure("ResearchNotFound");

                if (research.Participants == null)
                    return Result<Unit>.Failure("ResearchParticipantsIsEmpty.");

                _logger.LogInformation(
                    $"Number of participants in research with Id {request.ResearchId} is {research.Participants.Count}");

                // getting the panelmember using email claims in the jwt token
                var participant = await _dataContext.PanelMembers.FirstOrDefaultAsync(x =>
                        x.Email == _userAccessor.GetEmail());

                var participation = research.Participants
                    .FirstOrDefault(x => x.PanelMember.Email == participant?.Email);

                if (participation == null)
                    return Result<Unit>.Failure("ParticipantIsNotInResearch");

                // If the participant is found in the research, remove them
                research.Participants.Remove(participation);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result)
                {
                    _logger.LogError("Problem occurred while removing the selected participant. Save to the database failed.");
                    return Result<Unit>.Failure("defaultMessage");
                }

                _logger.LogInformation($"Successfully removed participant with {participant!.Email} from research {research.Title}.");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
