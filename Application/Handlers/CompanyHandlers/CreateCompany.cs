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
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                _dataContext.Add(request.Company);

                await _dataContext.SaveChangesAsync();
            }
        }
    }
}