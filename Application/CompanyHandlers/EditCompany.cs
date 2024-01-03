using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class EditCompany
    {

        public class Command : IRequest
        {
            public Company Company { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;
            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _databaseContext.Companies.FindAsync(request.Company.Id);
                _mapper.Map(request.Company, company);

                await _databaseContext.SaveChangesAsync();
            }
        }

    }
}