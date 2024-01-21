using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence.SeedData
{
    public static class Seed
    {
        public static async Task SeedAll(DataContext dataContext, UserManager<User> userManager)
        {
            await SeedUserBase.SeedUsers(userManager);
            await SeedUserBase.SeedCompanies(userManager);
            await SeedUserBase.SeedPanelMembers(userManager);
            await SeedChats.SeedChatBase(dataContext);
            await ResearchBase.SeedResearches(dataContext);
            await DisabilitiesBase.SeedDisabilites(dataContext);
        }
    }
}