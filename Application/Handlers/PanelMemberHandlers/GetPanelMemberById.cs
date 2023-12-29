using Domain;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class GetPanelMemberById
    {
        public class Query : IRequest<PanelMember>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, PanelMember>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }

            public async Task<PanelMember> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _databaseContext.PanelMembers.FindAsync(request.Id) ??
                    throw new Exception("PanelMember not found");
            }
        }
    }
}