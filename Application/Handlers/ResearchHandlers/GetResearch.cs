using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ResearchHandlers
{
    public class GetResearch
    {
        public class Query : IRequest<Result<List<Research>>> { }

        public class Handler : IRequestHandler<Query, Result<List<Research>>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Research> _logger;

            public Handler(DataContext dataContext, ILogger<Research> logger)
            {
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<List<Research>>> Handle(Query request, CancellationToken cancellationToken){
            _logger.LogInformation("Bezig met onderzoeken ophalen...");

                try
                {
                    var researches = await _dataContext.Researches.ToListAsync();

                    return Result<List<Research>>.Success(researches);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return Result<List<Research>>.Failure("Fout opgetreden bij ophalen van onderzoeken");
                }
            }
        }
    }
}
