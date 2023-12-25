using Domain;

namespace Persistence
{
    public class SeedUserBase
    {
        public static async Task SeedUsers(DatabaseContext context)
        {
            if (context.Users.Any()) return;

            var users = new List<User>
            {
                new User
                {
                    Email = "clodsire@pokemon.com",
                    Password = "bestpokemon",
                },
                new User
                {
                    Email = "charizard@pokemon.com",
                    Password = "firedragon",
                },
                new User
                {
                    Email = "mudkip@pokemon.com",
                    Password = "beststarter",
                },
                new User
                {
                    Email = "pikachu@pokemon.com",
                    Password = "pokemonmascot",
                },
                new User
                {
                    Email = "mew@pokemon.com",
                    Password = "firstpokemon",
                }
            };

            await context.Users.AddRangeAsync(users); // Save data into memory
            await context.SaveChangesAsync(); // Save changes into database
        }

        public static async Task SeedCompanies(DatabaseContext context)
        {
            if (context.Companies.Any()) return;

            var companies = new List<Company>
            {
                new Company
                {
                    Email = "company1@company.com",
                    Password = "company1",
                    Kvk = new Random().Next(00000000, 99999999).ToString(),
                    Name = "Bal.com",
                    Adres = "adres1",
                    Location = "Amsterdam",
                    Country = "Nederland",
                    Url = "www.bal.com",
                    Contact = "MBappe",
                },
                new Company
                {
                    Email = "company2@company.com",
                    Password = "company2",
                    Kvk = new Random().Next(00000000, 99999999).ToString(),
                    Name = "Coca-Loca",
                    Adres = "adres2",
                    Location = "New York",
                    Country = "USA",
                    Url = "www.Coca-Loca.com",
                    Contact = "Santa Clause",
                },
                new Company
                {
                    Email = "company3@company.com",
                    Password = "company3",
                    Kvk = new Random().Next(00000000, 99999999).ToString(),
                    Name = "Toyota",
                    Adres = "adres3",
                    Location = "Tokyo",
                    Country = "Japan",
                    Url = "www.toyota.com",
                    Contact = "Miyagi",
                },
                new Company
                {
                    Email = "company4@company.com",
                    Password = "company4",
                    Kvk = new Random().Next(00000000, 99999999).ToString(),
                    Name = "analytikyena",
                    Adres = "adres4",
                    Location = "Berlin",
                    Country = "Germany",
                    Url = "www.analytik-yena.com",
                    Contact = "Edward",
                },
                new Company
                {
                    Email = "company5@company.com",
                    Password = "company5",
                    Kvk = new Random().Next(00000000, 99999999).ToString(),
                    Name = "ClodsireClub",
                    Adres = "adres5",
                    Location = "Pallet Town",
                    Country = "Kanto",
                    Url = "www.clodsire-club.com",
                    Contact = "Ash Ketchum",
                }
            };

            await context.Companies.AddRangeAsync(companies); // Save data into memory
            await context.SaveChangesAsync(); // Save changes into database
        }

        public static async Task SeedPanelMembers(DatabaseContext context)
        {
            if (context.PanelMembers.Any()) return;

            var panelMembers = new List<PanelMember>
            {
                new PanelMember
                {
                    Email = "panelmember1@email.com",
                    Password = "panelmember1",
                    Guardian = new Random().Next(0, 100),
                    FirstName = "John",
                    LastName = "Doe",
                    Zipcode = "1234AB",
                    DateOfBirth = DateTime.UtcNow.AddMonths(1),
                },
                new PanelMember
                {
                    Email = "panelmember2@email.com",
                    Password = "panelmember2",
                    Guardian = new Random().Next(0, 100),
                    FirstName = "jane",
                    LastName = "Doe",
                    Zipcode = "1234Ac",
                    DateOfBirth = DateTime.UtcNow.AddMonths(2),
                },
                new PanelMember
                {
                    Email = "panelmember3@email.com",
                    Password = "panelmember3",
                    Guardian = new Random().Next(0, 100),
                    FirstName = "Paul",
                    LastName = "Doe",
                    Zipcode = "1234AD",
                    DateOfBirth = DateTime.UtcNow.AddMonths(3),
                },
                new PanelMember
                {
                    Email = "panelmember4@email.com",
                    Password = "panelmember4",
                    Guardian = new Random().Next(0, 100),
                    FirstName = "Bob",
                    LastName = "Ho",
                    Zipcode = "1234AE",
                    DateOfBirth = DateTime.UtcNow.AddMonths(4),
                },
                new PanelMember
                {
                    Email = "panelmember5@email.com",
                    Password = "panelmember5",
                    Guardian = new Random().Next(0, 100),
                    FirstName = "Might",
                    LastName = "Guy",
                    Zipcode = "1234AF",
                    DateOfBirth = DateTime.UtcNow.AddMonths(5),
                }
            };

            await context.PanelMembers.AddRangeAsync(panelMembers); // Save data into memory
            await context.SaveChangesAsync(); // Save changes into database
        }
    }
}