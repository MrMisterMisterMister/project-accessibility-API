using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class GetCompanyById
    {
        public class Query : IRequest<Result<Company>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Company>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Company>> Handle(Query request, CancellationToken cancellationToken)
            {
                var company = await _dataContext.Companies.FindAsync(request.Id.ToString());

                if (company == null) return Result<Company>.Failure("Company not found");

                return Result<Company>.Success(company);
            }
        }
    }
}