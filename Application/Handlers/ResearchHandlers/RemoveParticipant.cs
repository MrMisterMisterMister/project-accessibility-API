using Application.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Application.ResearchHandlers{
    public class RemoveParticipant{
        public class Command : IRequest<Result<Unit>>{
            public Guid ResearchId{get;set;}
            public Guid ParticipantId{get;set;}

        }
  public class Handler : IRequestHandler<Command, Result<Unit>>{
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;
            public Handler(DataContext dataContext, ILogger<Handler> logger){
                _dataContext = dataContext;
                _logger = logger;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken ){
                _logger.LogInformation("Bezig met ophalen van onderzoek...");
                try{
                var onderzoek = await _dataContext.Researches.FindAsync(request.ResearchId);
                
                if(onderzoek == null){
                    return Result<Unit>.Failure("Onderzoek niet gevonden");
                }
                if(onderzoek.Participants == null){
                    return Result<Unit>.Failure("Onderzoek bevat geen deelnemers.");
                }
                var deelnemerTeVewijderen = onderzoek.Participants.FirstOrDefault(p => p?.Id.ToString() == request.ParticipantId.ToString());
                if(deelnemerTeVewijderen == null){
                    return Result<Unit>.Failure("Deelnemer die verwijderd moet worden komt niet voor in het onderzoek of bestaat niet.");
                }
                //Als de deelnemer vooorkomt in het onderzoek wordt hij verwijderd
                onderzoek.Participants.Remove(deelnemerTeVewijderen);
                var resultaat = await _dataContext.SaveChangesAsync(cancellationToken) > 0;
                if(!resultaat){
                    return Result<Unit>.Failure("Probleem opgetreden bij het verwijderen van de geselecteerde deelnemer.");
                }
                _logger.LogInformation($"Succesvol deelnemer {deelnemerTeVewijderen.Id} verwijdert van onderzoek.");
                return Result<Unit>.Success(Unit.Value);
            
            }catch(Exception e){
                _logger.LogError("Er is een fout opgetreden bij het ophalen van het onderzoek", e.Message);
                return Result<Unit>.Failure("Er is een fout opgetreden tijdens het ophalen van het onderzoek.");
            }
            }
  }
    }
}