using Application.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ResearchHandlers{
    public class UpdateResearch{
        public class Command : IRequest<Result<Unit>>{
            public int ResearchId { get; set; }
            public string NieuweTitel { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>{
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext dataContext, ILogger<Handler> logger){
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken){
                _logger.LogInformation("Bezig met onderzoek aanpassen...");

                try{
                    var research = await _dataContext.Researches.FindAsync(request.ResearchId);

                    if (research == null){
                        return Result<Unit>.Failure("Onderzoek niet gevonden");
                    }
                    //Niet perse nodig maar zorgt ervoor dat er geen onnodige updates worden gemaakt naar onze database.
                    if(research.Title.Equals(request.NieuweTitel)){
                        return Result<Unit>.Failure("Titel is hetzelfde");
                    }

                    research.Title = request.NieuweTitel;
                    
                    var resultaat = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                    if (!resultaat){
                        return Result<Unit>.Failure("Fout opgetreden bij het aanpassen van het onderzoek.");
                    }

                    _logger.LogInformation($"Onderzoek '{request.ResearchId}' is aangepast met nieuwe naam '{request.NieuweTitel}'");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Er is een fout opgetreden bij het aanpassen van het onderzoek.");
                    return Result<Unit>.Failure("Er is een fout opgetreden bij het aanpassen van het onderzoek.");
                }
            }
        }
    }
}
