using Application.Core;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class DeleteCompany
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var company = await _dataContext.Companies.FindAsync(request.Id.ToString());

                if (company == null) return Result<Unit>.Failure("Panel member not found");

                _dataContext.Remove(company);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete the company");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}