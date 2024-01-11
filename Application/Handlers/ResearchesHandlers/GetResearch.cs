using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ResearchesHandlers
{
    public class GetResearch
    {
        public class Query : IRequest<Result<List<Research>>> {}

        public class Handler : IRequestHandler<Query, Result<List<Research>>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<List<Research>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Research>>.Success(await _dataContext.Researches.ToListAsync());
            }
        }
    }
}
