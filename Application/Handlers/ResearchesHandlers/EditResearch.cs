using Application.Core;
using MediatR;
using AutoMapper;
using Persistence;
using Domain;

namespace Application.ResearchesHandlers
{
    public class EditResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Research Research { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var research = await _dataContext.Researches.FindAsync(request.Research.Id);

                if (research == null) return Result<Unit>.Failure("ResearchNotFound");

                _mapper.Map(request.Research, research);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("ResearchedFailedUpdate");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
