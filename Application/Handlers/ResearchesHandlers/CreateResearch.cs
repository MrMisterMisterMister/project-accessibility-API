using Domain;
using Application.Core;
using MediatR;
using Persistence;
using Microsoft.Extensions.Logging;
using Application.Handlers.ResearchesHandlers;

namespace Application.ResearchesHandlers
{
    public class CreateResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public ResearchDTO Research { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext dataContext, ILogger<Handler> logger)
            {
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Research == null)
                    return Result<Unit>.Failure("ResearchNotFound");

                // Create a new Research entity with only the necessary properties
                var researchEntity = new Research
                {
                    Title = request.Research.Title,
                    Description = request.Research.Description,
                    Date = request.Research.Date,
                    Type = request.Research.Type,
                    Category = request.Research.Category,
                    Reward = request.Research.Reward,
                    OrganizerId = request.Research.OrganizerId
                };

                _dataContext.Add(researchEntity);

                var result = await _dataContext.SaveChangesAsync() > 0;
                Console.WriteLine($"Research creation completed: {result}");

                if (!result)
                    return Result<Unit>.Failure("ResearchFailedToBeCreated");

                _logger.LogInformation(
                    $"Successfully created research with title: {request.Research.Title} and id: {researchEntity.Id}");
                
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}