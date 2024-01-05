using Application.Core;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Application.ResearchHandlers{
    public class AddParticipant{
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
                _logger.LogInformation("Bezig met toevoegen van deelnemer...");
                try{
                    var onderzoek = await _dataContext.Researches.FindAsync(request.ResearchId);
                    if(onderzoek == null){
                        return Result<Unit>.Failure("Onderzoek bestaat niet.");
                    }
                    var deelnemer = await _dataContext.PanelMembers.FindAsync(request.ParticipantId);

                    if(deelnemer == null){
                        return Result<Unit>.Failure("Deelnemer bestaat niet.");
                    }
                    //Check of de deelnemer al voorkomt in het onderzoek of niet (voorkomt dubbele)
                   if (onderzoek.Participants?.Any(deelnemer => deelnemer?.Id.ToString() == request.ParticipantId.ToString()) == true){
                    return Result<Unit>.Failure($"Deelnemer {deelnemer.UserName} komt al voor in het onderzoek");
                    }
                    //Ze zeggen hier mogelijke null reference maar dat kan niet, je checkt namelijk bij lijn 35 al of de deelnemer bestaat of niet, er wordt hier dus altijd een deelnemer toegevoegd.
                    //Ik onderdruk daarom ook de waarschuwing omdat ik het niet mooi vind staan.
#pragma warning disable CS8602 
                    onderzoek.Participants.Add(deelnemer);
#pragma warning restore CS8602

                    //resultaat is true als er changes zijn opgeslagen en false als er geen zijn opgeslagen.
                    bool resultaat = await _dataContext.SaveChangesAsync(cancellationToken) > 0;
                    if(!resultaat){
                    return Result<Unit>.Failure($"Probleem opgetreden bij het toevoegen van de deelnemer. Id: {request.ParticipantId}");
                    }
                    _logger.LogInformation($"Succesvol deelnemer {deelnemer.UserName} toegevoegd aan onderzoek");
                    return Result<Unit>.Success(Unit.Value);
                }catch(Exception e){
                    _logger.LogError("Fout opgetreden bij ophalen van onderzoek", e.Message);
                    return Result<Unit>.Failure("Fout opgetreden bij ophalen van onderzoek");
                }
            }
  }
    }
}