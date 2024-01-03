using Application.Core;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ResearchHandlers
{
    public class CreateResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Research Research { get; set; } = null!;
        }

        // Moet nog wel ff ervoor zorgen dat alleen een bedrijf een onderzoek kan starten, als de rbac correct werkt implementeer ik dit.
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
                _logger.LogInformation("Bezig met onderzoek aanmaken...");

                try{
                    _dataContext.Add(request.Research);

                    var resultaat = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                    if (!resultaat)
                    {
                        return Result<Unit>.Failure("Fout opgetreden bij het maken van het onderzoek.");
                    }

                    _logger.LogInformation($"Onderzoek '{request.Research.Title}' is aangemaakt");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Er is een fout opgetreden bij het aanmaken van het onderzoek.");
                    return Result<Unit>.Failure("Er is een fout opgetreden bij het aanmaken van het onderzoek.");
                }
            }
        }
    }
}
