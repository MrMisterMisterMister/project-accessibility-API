using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class GetPanelMemberById
    {
        public class Query : IRequest<Result<PanelMember>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PanelMember>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<PanelMember>> Handle(Query request, CancellationToken cancellationToken)
            {
                var panelMember = await _dataContext.PanelMembers.FindAsync(request.Id.ToString());

                if (panelMember == null) return Result<PanelMember>.Failure("Panel member not found");

                return Result<PanelMember>.Success(panelMember);
            }
        }
    }
}