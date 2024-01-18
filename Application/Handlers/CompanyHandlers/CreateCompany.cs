using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class CreateCompany
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Company Company { get; set; } = null!;
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
                _dataContext.Add(request.Company);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("CompanyFailedCreate", "Failed to create company.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}