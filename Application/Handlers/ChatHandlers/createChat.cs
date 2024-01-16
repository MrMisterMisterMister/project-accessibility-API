using Domain;
using Persistence;
using MediatR;
using Application.Core;
using Domain.Models.ChatModels;
namespace Application.ChatHandlers{
    public class createChat{
        public class Command : IRequest<Result<Unit>>{
            public required string User1 {get;set;}
            public required string User2 {get;set;}

        }
        public class Handler : IRequest<Result<Unit>>{
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext){
                _dataContext = dataContext;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken){
                var user1 = await _dataContext.Users.FindAsync(request.User1);
                var user2 = await _dataContext.Users.FindAsync(request.User2);
                
                if(user1 == null || user2 == null){
                    return Result<Unit>.Failure("One or more users do not exist");
                }
                var newChat = new Chat{
                    User1Id = request.User1,
                    User2Id = request.User2
            };
            _dataContext.Chats.Add(newChat);
            var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

            if(result){
         return Result<Unit>.Success(Unit.Value);
                }else{
                    return Result<Unit>.Failure("Failed to create chat");
                }
            }
        }
            }
            }
    