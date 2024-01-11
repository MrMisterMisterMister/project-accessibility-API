using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.ResearchesHandlers
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

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Research>> Handle(Query request, CancellationToken cancellationToken)
            {
                var research = await _dataContext.Researches.FindAsync(request.ResearchId);

                if (research == null) return Result<Research>.Failure("Research not found");

                return Result<Research>.Success(research);
            }
        }
    }
}
