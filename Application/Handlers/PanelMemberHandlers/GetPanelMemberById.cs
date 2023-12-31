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
            private readonly DataContext _dateContext;
            public Handler(DataContext dataContext)
            {
                _dateContext = dataContext;
            }

            public async Task<PanelMember> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _dateContext.PanelMembers.FindAsync(request.Id) ??
                    throw new Exception("PanelMember not found");
            }
        }
    }
}