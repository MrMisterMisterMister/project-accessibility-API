using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class GetPanelMember
    {
        public class Query : IRequest<Result<List<PanelMember>>> {}

        public class Handler : IRequestHandler<Query, Result<List<PanelMember>>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<List<PanelMember>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<PanelMember>>.Success(await _dataContext.PanelMembers.ToListAsync());
            }
        }
    }
}