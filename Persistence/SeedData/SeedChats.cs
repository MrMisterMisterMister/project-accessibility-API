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
                    User1Id = panelmembers[1].Id,
                    User2Id = panelmembers[2].Id,
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
             User1Id = panelmembers[1].Id,
             User2Id = panelmembers[2].Id,
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
                       User1Id = panelmembers[1].Id,
                       User2Id = panelmembers[2].Id,
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
