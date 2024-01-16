using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain.Models.ChatModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.MessageHandlers{
    public class SendMessage{
        public class Command : IRequest<Result<Unit>>{
            public int ChatId { get; set; } 
            public required string Content { get; set; } 
            public required string SenderId { get; set; } 
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>{
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext){
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken){
                var chat = await _dataContext.Chats.FindAsync(request.ChatId);
                if (chat == null){
                    return Result<Unit>.Failure("Chat not found");
                }

                var message = new Message{
                    Content = request.Content,
                    SenderId = request.SenderId,
                };
                _dataContext.Messages.Add(message);

                chat.Messages ??= new List<Message>();
                chat.Messages.Add(message);

                try{
                    await _dataContext.SaveChangesAsync(cancellationToken);
                    return Result<Unit>.Success(Unit.Value);

                }catch(DbUpdateConcurrencyException){
                    await _dataContext.Entry(message).ReloadAsync();
                    return Result<Unit>.Failure("Concurrency conflict.");
                    
                }catch(Exception ex){
                return Result<Unit>.Failure($"Error sending message: {ex.Message}");

                }
                }
            }
            
        }
    }
