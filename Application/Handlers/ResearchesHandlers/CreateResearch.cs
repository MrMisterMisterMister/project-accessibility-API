using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.ResearchesHandlers
{
    public class CreateResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Research Research { get; set; } = null!;
            public Company Organizer { get; set; } = null!;
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
                // Add organizer
                request.Research.Organizer = request.Organizer;

                _dataContext.Add(request.Research);

                // resultaat is true als er changes zijn opgeslagen en false als er geen zijn opgeslagen.
                var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to create research");    
            
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}