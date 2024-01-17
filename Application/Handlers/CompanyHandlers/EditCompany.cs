using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class EditCompany
    {

        public class Command : IRequest<Result<Unit>>
        {
            public Company Company { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _dataContext.Companies.FindAsync(request.Company.Id);

                if (company == null) return Result<Unit>.Failure("CompanyNotFound", "Company could not be found.");

                request.Company.UserName = company.UserName;
                request.Company.NormalizedUserName = company.NormalizedUserName;
                request.Company.Email = company.Email;
                request.Company.NormalizedEmail = company.NormalizedEmail;
                request.Company.PasswordHash = company.PasswordHash;

                _mapper.Map(request.Company, company);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("CompanyFailedUpdate", "Failed to update company.");

                return Result<Unit>.Success(Unit.Value);
            }
        }

    }
}