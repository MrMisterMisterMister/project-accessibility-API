using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class GetPanelMember
    {
        public class Query : IRequest<List<PanelMember>> { }

        public class Handler : IRequestHandler<Query, List<PanelMember>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<List<PanelMember>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _dataContext.PanelMembers.ToListAsync();
            }
        }
    }
}