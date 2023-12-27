using Domain;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class CreatePanelMember
    {
        public class Command : IRequest
        {
            public PanelMember PanelMember { get; set; } = null!;
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
                _databaseContext.Add(request.PanelMember);

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}