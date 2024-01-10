using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ResearchHandlers{
    public class DeleteResearch{
        public class Command : IRequest<Result<Unit>>{
            public int ResearchId  { get; set; }
        }
        //Moet nog wel ff ervoor zorgen dat alleen een bedrijf een onderzoek kan verwijderen, als rbac correct werkt doe ik dit ff.
        public class Handler : IRequestHandler<Command, Result<Unit>>{
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;
            public Handler(DataContext dataContext, ILogger<Handler> logger){
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken){
            _logger.LogInformation("Bezig met onderzoek verwijderen...");

            try{
                var research = await _dataContext.Researches.FindAsync(request.ResearchId );
                if (research == null){
                    return Result<Unit>.Failure("Onderzoek niet gevonden.");
                }

                _dataContext.Remove(research);
                
                //resultaat is true als er changes zijn opgeslagen en false als er geen zijn opgeslagen.
                bool result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                if (!result){
                    return Result<Unit>.Failure("Fout opgetreden bij het verwijderen van het onderzoek.");
                }
                _logger.LogInformation($"Het onderzoek {research.Id} is succesvol verwijderd!");
                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception e){
                _logger.LogError("Er is een fout opgetreden bij het verwijderen of ophalen van het onderzoek.", e.Message);
                return Result<Unit>.Failure("Er is een fout opgetreden bij het verwijderen of ophalen van het onderzoek.");
            }
        }
    }
    }
}