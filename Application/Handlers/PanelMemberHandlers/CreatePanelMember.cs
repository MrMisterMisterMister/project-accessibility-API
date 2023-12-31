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
            private readonly DataContext _dateContext;
            public Handler(DataContext dataContext)
            {
                _dateContext = dataContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                _dateContext.Add(request.PanelMember);

                await _dateContext.SaveChangesAsync();
            }
        }
    }
}