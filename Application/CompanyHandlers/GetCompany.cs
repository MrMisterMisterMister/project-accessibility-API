using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CompanyHandlers
{
    public class GetCompany
    {
        public class Query : IRequest<List<Company>> { }

        public class Handler : IRequestHandler<Query, List<Company>>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }

            public async Task<List<Company>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _databaseContext.Companies.ToListAsync();
            }
        }
    }
}