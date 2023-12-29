using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class CreateCompany
    {
        public class Command : IRequest
        {
            public Company Company { get; set; } = null!;
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
                _databaseContext.Add(request.Company);

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}