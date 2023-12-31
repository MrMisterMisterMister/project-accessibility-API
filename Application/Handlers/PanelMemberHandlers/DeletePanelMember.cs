using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class DeletePanelMember
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var panelMember = await _dataContext.PanelMembers.FindAsync(request.Id) ??
                    throw new Exception("PanelMember not found");

                _dataContext.Remove(panelMember);

                await _dataContext.SaveChangesAsync();
            }
        }
    }
}