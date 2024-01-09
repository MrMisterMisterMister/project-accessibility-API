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

namespace Application.ResearchHandlers{
    public class ParticipateInResearch{
        public class Command : IRequest<Result<Unit>>
        {
            public int ResearchId { get; set; }
            public Guid ParticipantId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>{
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext dataContext, ILogger<Handler> logger){
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken){
                _logger.LogInformation($"Bezig met inschrijven voor onderzoek voor deelnemer: {request.ParticipantId}...");

                try{
                    var research = await _dataContext.Researches
                        .Include(r => r.Participants) //Ik maak hier gebruik van eager loading voor sneller process.
                        .FirstOrDefaultAsync(r => r.Id == request.ResearchId, cancellationToken);

                    if (research == null){
                        return Result<Unit>.Failure("Onderzoek niet gevonden.");
                    }

                    var participant = await _dataContext.PanelMembers
                        .FirstOrDefaultAsync(p => p.Id.ToString() == request.ParticipantId.ToString(), cancellationToken);

                    if (participant == null){
                        return Result<Unit>.Failure("Deelnemer niet gevonden.");
                    }

                    if (research.Participants?.Any(p => p?.Id == participant.Id) == true){
                        return Result<Unit>.Failure("Deelnemer is al ingeschreven voor dit onderzoek.");
                    }
                    //Ze zeggen mogelijke null reference hier maar kan niet omdat je op lijn 44 al checkt of ie null is.
                    research.Participants.Add(participant);

                    var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                    if (!result){
                        return Result<Unit>.Failure("Fout opgetreden bij het inschrijven voor het onderzoek.");
                    }

                    _logger.LogInformation($"Deelnemer {participant.UserName} succesvol ingeschreven voor onderzoek '{research.Title}'.");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception e){
                    _logger.LogError(e, "Er is een fout opgetreden bij het inschrijven voor het onderzoek.");
                    return Result<Unit>.Failure("Er is een fout opgetreden bij het inschrijven voor het onderzoek.");
                }
            }
        }
    }
}
