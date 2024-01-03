using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CompanyHandlers
{
    public class GetCompany
    {
        public class Query : IRequest<Result<List<Company>>> { }

        public class Handler : IRequestHandler<Query, Result<List<Company>>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<List<Company>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Company>>.Success(await _dataContext.Companies.ToListAsync());
            }
        }
    }
}