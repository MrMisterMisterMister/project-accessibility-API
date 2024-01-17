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
            public required string Title {get;set;}

        }
        public class Handler : IRequestHandler<Command, Result<Unit>>{
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
                var existingChat = _dataContext.Chats
                    .FirstOrDefault(chat =>
                        (chat.User1Id == request.User1 && chat.User2Id == request.User2) ||
                        (chat.User1Id == request.User2 && chat.User2Id == request.User1));

                if (existingChat != null)
                {
                    return Result<Unit>.Failure("Chat already exists between these users");
                }
                if(request.User1.Equals(request.User2)){
                return Result<Unit>.Failure("Chat cant contain the id of the same users.");
                }
                var newChat = new Chat{
                    User1Id = request.User1,
                    User2Id = request.User2,
                    User1Email = user1.Email,
                    User2Email = user2.Email
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