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
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }

            public async Task<List<PanelMember>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _databaseContext.PanelMembers.ToListAsync();
            }
        }
    }
}