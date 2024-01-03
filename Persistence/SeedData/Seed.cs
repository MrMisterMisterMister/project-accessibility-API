namespace Persistence.SeedData
{
    public static class Seed
    {
        public static async Task SeedAll(DatabaseContext databaseContext)
        {
            await SeedUserBase.SeedUsers(databaseContext);
            await SeedUserBase.SeedCompanies(databaseContext);
            await SeedUserBase.SeedPanelMembers(databaseContext);
        }
    }
}