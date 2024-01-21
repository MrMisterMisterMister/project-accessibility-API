using Domain;
using Domain.Models.ChatModels;
using Microsoft.EntityFrameworkCore;
namespace Persistence.SeedData{
    public class SeedChats{
        public static async Task SeedChatBase(DataContext dataContext){
            var panelmembers = await dataContext.Users.OfType<PanelMember>().ToListAsync();

            if(!panelmembers.Any()){
                return;
            }
            if(!dataContext.Chats.Any()){
            var chats = new List<Chat>{
                new Chat{
                    Title = "Chat 1",
                    User1Id = panelmembers[1].Id,
                    User2Id = panelmembers[2].Id,
                    User1Email = panelmembers[1].Email,
                    User2Email = panelmembers[2].Email,
                    Messages = new List<Message>(){
                        new Message{
                            SenderId = panelmembers[1].Id,
                            Content = "Hallo, dit is een test bericht",
                            Timestamp = DateTime.UtcNow,
                            IsRead = false

                        }             
                    }
            },
            new Chat{
             Title = "Chat 2",
             User1Id = panelmembers[1].Id,
             User2Id = panelmembers[2].Id,
            User1Email = panelmembers[1].Email,
            User2Email = panelmembers[2].Email,
                    Messages = new List<Message>(){
                        new Message{
                            SenderId = panelmembers[3].Id,
                            Content = "Dit is nog een test bericht.",
                            Timestamp = DateTime.UtcNow,
                            IsRead = true
                        }
                    }
            },
               new Chat{
                       Title = "Chat 3",
                       User1Id = panelmembers[1].Id,
                       User2Id = panelmembers[2].Id,
                    User1Email = panelmembers[1].Email,
                    User2Email = panelmembers[2].Email,
                       Messages = new List<Message>(){
                        new Message{
                            SenderId = panelmembers[1].Id,
                            Content = "Dit is alweer een test bericht.",
                            Timestamp = DateTime.UtcNow,
                            IsRead = false
}
                       }
               }
            };
            await dataContext.Chats.AddRangeAsync(chats);
            await dataContext.SaveChangesAsync();
        }
    }
}
}
