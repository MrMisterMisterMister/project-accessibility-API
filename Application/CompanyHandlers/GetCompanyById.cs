using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class GetCompanyById
    {
        public class Query : IRequest<Company>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Company>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }

            public async Task<Company> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _databaseContext.Companies.FindAsync(request.Id) ??
                    throw new Exception("Company not found");
            }
        }
    }
}