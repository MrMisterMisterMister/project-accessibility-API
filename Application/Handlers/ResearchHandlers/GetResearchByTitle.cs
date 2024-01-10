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
        _logger.LogInformation("Bezig met onderzoek opzoeken op titel...");

        try
        {
         var research = await _dataContext.Researches
                .FirstOrDefaultAsync(r => r.Title == request.Title, cancellationToken);

            if (research == null){
                        return Result<Research>.Failure("Onderzoek niet gevonden.");
            }

            _logger.LogInformation($"Onderzoek met titel '{request.Title}' is opgehaald.");
                    return Result<Research>.Success(research);
        }
        catch (Exception e)
        {
            _logger.LogError("Er is een fout opgetreden bij het opzoeken van het onderzoek op titel.", e.Message);
                    return Result<Research>.Failure("Er is een fout opgetreden bij het opzoeken van het onderzoek op titel.");
        }
    }
}
    }
}