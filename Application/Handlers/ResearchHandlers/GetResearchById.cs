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
    public class GetResearchById
    {
        public class Query : IRequest<Result<Research>>
        {
            public int ResearchId { get; set; }
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
                _logger.LogInformation("Bezig met onderzoek opzoeken op ID...");

                try
                {
                    var research = await _dataContext.Researches.FindAsync(request.ResearchId);

                    if (research == null)
                    {
                        return Result<Research>.Failure("Onderzoek niet gevonden.");
                    }

                    _logger.LogInformation($"Onderzoek met ID {request.ResearchId} is opgehaald.");
                    return Result<Research>.Success(research);
                }
                catch (Exception e)
                {
                    _logger.LogError("Er is een fout opgetreden bij het opzoeken van het onderzoek op ID.", e.Message);
                    return Result<Research>.Failure("Er is een fout opgetreden bij het opzoeken van het onderzoek op ID.");
                }
            }
        }
    }
}
