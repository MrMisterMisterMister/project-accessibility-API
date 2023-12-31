using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class EditUser
    {

        public class Command : IRequest<Result<Unit>>
        {
            public User User { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            // Injects DataContext and IMapper (so we dont have to put each user property in the Handle method)
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FindAsync(request.User.Id);

                if (user == null) return Result<Unit>.Failure("user not found");

                _mapper.Map(request.User, user);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }

    }
}