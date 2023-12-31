using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class CreatePanelMember
    {
        public class Command : IRequest<Result<Unit>>
        {
            public PanelMember PanelMember { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _dataContext.Add(request.PanelMember);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create panel member");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}