using Domain;
using Application.Core;
using MediatR;
using Persistence;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.ResearchHandlers
{
    public class CreateResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Research Research { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, ILogger<Handler> logger, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                // getting the company using email claims in the jwt token
                var organizer = await _dataContext.Companies.FirstOrDefaultAsync(x =>
                        x.Email == _userAccessor.GetEmail());

                request.Research.Organizer = organizer;

                _dataContext.Add(request.Research);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("ResearchFailedToBeCreated", "The research could not be created.");

                _logger.LogInformation(
                    $"Successfully created research with title: {request.Research.Title} and id: {request.Research.Id}");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}