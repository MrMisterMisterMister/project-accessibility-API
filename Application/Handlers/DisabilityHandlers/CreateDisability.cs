using Application.Core;
using Domain.Models.Disabilities;
using MediatR;
using Persistence;

namespace Application.DisabilityHandlers
{
    public class CreateDisability
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Disability Disability { get; set; } = null!;
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
                _dataContext.Add(request.Disability);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("DisabilityFailedCreate", "Failed to create disability.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}