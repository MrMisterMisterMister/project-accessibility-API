using Application.Core;
using MediatR;
using Persistence;

namespace Application.ResearchesHandlers
{
    public class DeleteResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ResearchId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var research = await _dataContext.Researches.FindAsync(request.ResearchId);

                if (research == null) return Result<Unit>.Failure("ResearchNotFound");

                _dataContext.Remove(research);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("ResearchFailedDelete");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}