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
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var panelMember = await _databaseContext.PanelMembers.FindAsync(request.Id) ??
                    throw new Exception("PanelMember not found");

                _databaseContext.Remove(panelMember);

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}