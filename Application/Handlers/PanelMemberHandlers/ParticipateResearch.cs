using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ResearchHandlers
{
    public class ParticipateInResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ResearchId { get; set; }
            public Guid ParticipantId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext dataContext, ILogger<Handler> logger){
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken){
                _logger.LogInformation($"Bezig met inschrijven voor onderzoek voor deelnemer: {request.ParticipantId}...");

                try {
                    var research = await _dataContext.Researches
                        .Include(r => r.Participants)
                        .FirstOrDefaultAsync(r => r.Id == request.ResearchId, cancellationToken);

                    if (research == null){
                        return Result<Unit>.Failure("Onderzoek niet gevonden of bestaat niet.");
                    }

                    var participant = await _dataContext.PanelMembers
                        .FirstOrDefaultAsync(p => p.Id == request.ParticipantId.ToString(), cancellationToken);

                    if (participant == null){
                        return Result<Unit>.Failure("Deelnemer niet gevonden.");
                    }

                    if (research.Participants.Any(p => p.PanelMemberId == request.ParticipantId)){
                        return Result<Unit>.Failure("Deelnemer is al ingeschreven voor dit onderzoek.");
                    }

                    research.Participants.Add(new Participant { PanelMemberId = request.ParticipantId });

                    var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                    if (!result){
                        return Result<Unit>.Failure("Fout opgetreden bij het inschrijven voor het onderzoek.");
                    }

                    _logger.LogInformation($"Deelnemer {participant.UserName} succesvol ingeschreven voor onderzoek '{research.Title}'.");
                    return Result<Unit>.Success(Unit.Value);
                }catch (Exception e){
                    _logger.LogError(e, "Er is een fout opgetreden bij het inschrijven voor het onderzoek.");
                    return Result<Unit>.Failure("Er is een fout opgetreden bij het inschrijven voor het onderzoek.");
                }
            }
        }
    }
}
