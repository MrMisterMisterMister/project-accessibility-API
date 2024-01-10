using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ResearchHandlers
{
    public class GetResearchByTitle
    {
        public class Query : IRequest<Result<Research>>
        {
            public string Title { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Query, Result<Research>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext dataContext, ILogger<Handler> logger)
            {
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Research>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("Bezig met ophalen van onderzoek op titel...");

                    if (string.IsNullOrWhiteSpace(request.Title))
                    {
                        _logger.LogWarning("Titel parameter is leeg of null.");
                        return Result<Research>.Failure("Titel parameter is vereist.");
                    }
                    var research = await _dataContext.Researches
                        .FirstOrDefaultAsync(r => r.Title == request.Title.Trim(), cancellationToken);

                    if (research == null)
                    {
                        _logger.LogWarning($"Onderzoek met titel '{request.Title.Trim()}' niet gevonden.");
                        return Result<Research>.Failure("Onderzoek niet gevonden.");
                    }

                    _logger.LogInformation($"Onderzoek met titel '{request.Title.Trim()}' succesvol opgehaald.");
                    return Result<Research>.Success(research);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Er is een fout opgetreden bij het ophalen van het onderzoek op titel: {ex.Message}");
                    return Result<Research>.Failure("Er is een fout opgetreden bij het ophalen van het onderzoek op titel.");
                }
            }
        }
    }
}
