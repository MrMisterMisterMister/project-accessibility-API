using Application.Core;
using MediatR;
using Persistence;

namespace Application.ResearchesHandlers
{
    public class DeleteDisability
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int DisabilityId { get; set; }
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
                var disability = await _dataContext.Disabilities.FindAsync(request.DisabilityId);

                if (disability == null) return Result<Unit>.Failure("DisabilityNotFound", "The disability could not be found.");

                _dataContext.Remove(disability);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("DisabilityFailedDelete", "The disability could not be deleted.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}