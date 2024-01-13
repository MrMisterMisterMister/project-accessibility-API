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
                try
                {
                    if (request.Research == null)
                    {
                        return Result<Unit>.Failure("Ongeldig verzoek, het onderzoeksobject mag niet leeg zijn.");
                    }

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

                    var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;
                    Console.WriteLine($"Onderzoek aanmaken voltooid: {result}");

                    if (!result)
                    {
                        return Result<Unit>.Failure("Mislukt om onderzoek aan te maken");
                    }

                    _logger.LogInformation($"Succesvol onderzoek aangemaakt met titel: {request.Research.Title} en id: {researchEntity.Id}");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure($"Er is een fout opgetreden: {ex.Message}");
                }
            }
        }
    }
}