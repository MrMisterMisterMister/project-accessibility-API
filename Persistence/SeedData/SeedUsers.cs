using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class SeedUserBase
    {
        public static async Task SeedUsers(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com"
                    },
                    new User {
                        UserName = "user@test.com",
                        Email = "user@test.com"
                    },
                    new User {
                        UserName = "clodsire@pokemon.com",
                        Email = "clodsire@pokemon.com"
                    },
                    new User {
                        UserName = "bidoof@pokemon.com",
                        Email = "bidoof@pokemon.com"
                    },
                    new User {
                        UserName = "terastal@pokemon.com",
                        Email = "terastal@pokemon.com"
                    },
                    new User {
                        UserName = "karel@pokemon.com",
                        Email = "karel@pokemon.com"
                    },
                };

                foreach (var user in users)
                {
                    // Creates and saves user with the given password in datababase
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }

        public static async Task SeedCompanies(UserManager<User> userManager)
        {
            if (!userManager.Users.OfType<Company>().Any())
            {
                var companies = new List<Company>
                {
                    new Company
                    {
                        Email = "company1@company.com",
                        UserName = "company1@company.com",
                        Kvk = new Random().Next(00000000, 99999999).ToString(),
                        CompanyName = "Bal.com",
                        Phone = "12345678",
                        Address = "adres1",
                        PostalCode = "1234AB",
                        Province = "Amsterdam",
                        Country = "Nederland",
                        WebsiteUrl = "www.bal.com",
                        ContactPerson = "MBappe",
                    },
                    new Company
                    {
                        Email = "company2@company.com",
                        UserName = "company2@company.com",
                        Kvk = new Random().Next(00000000, 99999999).ToString(),
                        CompanyName = "Coca-Loca",
                        Phone = "88855888",
                        Address = "adres2",
                        PostalCode = "4321BA",
                        Province = "New York",
                        Country = "USA",
                        WebsiteUrl = "www.Coca-Loca.com",
                        ContactPerson = "Santa Clause",
                    },
                    new Company
                    {
                        Email = "company3@company.com",
                        UserName = "company3@company.com",
                        Kvk = new Random().Next(00000000, 99999999).ToString(),
                        CompanyName = "Toyota",
                        Phone = "91923923",
                        Address = "adres3",
                        PostalCode = "9876ZC",
                        Province = "Tokyo",
                        Country = "Japan",
                        WebsiteUrl = "www.toyota.com",
                        ContactPerson = "Miyagi",
                    },
                    new Company
                    {
                        Email = "company4@company.com",
                        UserName = "company4@company.com",
                        Kvk = new Random().Next(00000000, 99999999).ToString(),
                        CompanyName = "analytikyena",
                        Phone = "12495953",
                        Address = "adres4",
                        PostalCode = "6666PP",
                        Province = "Berlin",
                        Country = "Germany",
                        WebsiteUrl = "www.analytik-yena.com",
                        ContactPerson = "Edward",
                    },
                    new Company
                    {
                        Email = "company5@company.com",
                        UserName = "company5@company.com",
                        Kvk = new Random().Next(00000000, 99999999).ToString(),
                        CompanyName = "ClodsireClub",
                        Phone = "19491422",
                        Address = "adres5",
                        PostalCode = "6969EX",
                        Province = "Pallet Town",
                        Country = "Kanto",
                        WebsiteUrl = "www.clodsire-club.com",
                        ContactPerson = "Ash Ketchum",
                    }
                };

                foreach (var company in companies)
                {
                    // Creates and saves user with the given password in datababase
                    await userManager.CreateAsync(company, "Pa$$w0rd");
                }
            }
        }

        public static async Task SeedPanelMembers(UserManager<User> userManager)
        {
            if (!userManager.Users.OfType<PanelMember>().Any())
            {
                var panelMembers = new List<PanelMember>
                {
                    new PanelMember
                    {
                        Email = "panelmember1@email.com",
                        UserName = "panelmember1@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "John",
                        LastName = "Doe",
                        DateOfBirth = DateTime.UtcNow.AddMonths(1),
                        Address = "I live in your head",
                        PostalCode = "9584BR",
                        City = "Muckanaghederdauhaulia",
                        Country = "Africa",
                    },
                    new PanelMember
                    {
                        Email = "panelmember2@email.com",
                        UserName = "panelmember2@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "Jane",
                        LastName = "Doe",
                        DateOfBirth = DateTime.UtcNow.AddMonths(2),
                        Address = "Johanna Westerdijkplein 75",
                        PostalCode = "2521EN",
                        City = "Den Haag",
                        Country = "The Netherlands",
                    },
                    new PanelMember
                    {
                        Email = "panelmember3@email.com",
                        UserName = "panelmember3@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "Paul",
                        LastName = "Doe",
                        DateOfBirth = DateTime.UtcNow.AddMonths(3),
                        Address = "Eyjafjallajokull 54",
                        PostalCode = "9184AZ",
                        City = "Middle of Nowhere",
                        Country = "Your Backyard",
                    },
                    new PanelMember
                    {
                        Email = "panelmember4@email.com",
                        UserName = "panelmember4@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "Bob",
                        LastName = "Ho",
                        DateOfBirth = DateTime.UtcNow.AddMonths(4),
                        Address = "Trashbin street 85",
                        PostalCode = "9194MA",
                        City = "Bisolavska",
                        Country = "Russia",
                    },
                    new PanelMember
                    {
                        Email = "panelmember5@email.com",
                        UserName = "panelmember5@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "Might",
                        LastName = "Guy",
                        DateOfBirth = DateTime.UtcNow.AddMonths(5),
                        Address = "Route 20 and 21",
                        PostalCode = "7777MM",
                        City = "Cinnabar Island",
                        Country = "Kanto",
                    }
                };

                foreach (var panelMember in panelMembers)
                {
                    // Creates and saves user with the given password in datababase
                    await userManager.CreateAsync(panelMember, "Pa$$w0rd");
                }
            }
        }
    }
}