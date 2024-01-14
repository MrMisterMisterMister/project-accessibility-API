using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ParticipantsHandlers
{
     public class ParticipateInResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Research { get; set; }
            public string Participant { get; set; } = null!;
            public DateTime DateJoined { get; set; } 
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext dataContext, ILogger<Handler> logger)
            {
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Bezig met inschrijven voor onderzoek voor deelnemer: {request.Participant}...");

                try
                {
                    var research = await _dataContext.Researches
                        .Include(r => r.Participants)
                        .FirstOrDefaultAsync(r => r.Id == request.Research, cancellationToken);

                    if (research == null)
                    {
                        return Result<Unit>.Failure("Onderzoek niet gevonden of bestaat niet.");
                    }
                    _logger.LogInformation($"Onderzoek: {research.Id} gevonden!");

                    var participant = await _dataContext.PanelMembers
                        .FirstOrDefaultAsync(p => p.Id == request.Participant, cancellationToken);

                    if (participant == null)
                    {
                        return Result<Unit>.Failure("Deelnemer niet gevonden.");
                    }
                    _logger.LogInformation($"Deelnemer: {participant.Id} gevonden!");

                    if (research.Participants.Any(p => p.PanelMemberId == request.Participant))
                    {
                        return Result<Unit>.Failure("Deelnemer is al ingeschreven voor dit onderzoek.");
                    }

                    research.Participants.Add(new ResearchParticipant { PanelMemberId = request.Participant, DateJoined = request.DateJoined });

                    var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                    if (!result)
                    {
                        return Result<Unit>.Failure("Fout opgetreden bij het inschrijven voor het onderzoek.");
                    }

                    _logger.LogInformation($"Deelnemer {participant.UserName} succesvol ingeschreven voor onderzoek '{research.Title}'.");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Er is een fout opgetreden bij het inschrijven voor het onderzoek.");
                    return Result<Unit>.Failure("Er is een fout opgetreden bij het inschrijven voor het onderzoek.");
                }
            }
        }
    }
}