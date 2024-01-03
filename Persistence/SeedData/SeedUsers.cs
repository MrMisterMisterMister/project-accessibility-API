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
                    new User{UserName = "user@test.com", Email = "user@test.com"},
                    new User{UserName = "clodsire@pokemon", Email = "clodsire@pokemon"},
                    new User{UserName = "bidoof@pokemon", Email = "bidoof@pokemon"},
                    new User{UserName = "terastal@pokemon", Email = "terastal@pokemon"},
                    new User{UserName = "karel@pokemon", Email = "karel@pokemon"},
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
                        UserName = "company2@company.com",
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
                        UserName = "company3@company.com",
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
                        UserName = "company4@company.com",
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
                        UserName = "company5@company.com",
                        Kvk = new Random().Next(00000000, 99999999).ToString(),
                        Name = "ClodsireClub",
                        Adres = "adres5",
                        Location = "Pallet Town",
                        Country = "Kanto",
                        Url = "www.clodsire-club.com",
                        Contact = "Ash Ketchum",
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
                        Zipcode = "1234AB",
                        DateOfBirth = DateTime.UtcNow.AddMonths(1),
                    },
                    new PanelMember
                    {
                        Email = "panelmember2@email.com",
                        UserName = "panelmember2@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "jane",
                        LastName = "Doe",
                        Zipcode = "1234Ac",
                        DateOfBirth = DateTime.UtcNow.AddMonths(2),
                    },
                    new PanelMember
                    {
                        Email = "panelmember3@email.com",
                        UserName = "panelmember3@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "Paul",
                        LastName = "Doe",
                        Zipcode = "1234AD",
                        DateOfBirth = DateTime.UtcNow.AddMonths(3),
                    },
                    new PanelMember
                    {
                        Email = "panelmember4@email.com",
                        UserName = "panelmember4@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "Bob",
                        LastName = "Ho",
                        Zipcode = "1234AE",
                        DateOfBirth = DateTime.UtcNow.AddMonths(4),
                    },
                    new PanelMember
                    {
                        Email = "panelmember5@email.com",
                        UserName = "panelmember5@email.com",
                        Guardian = new Random().Next(0, 100),
                        FirstName = "Might",
                        LastName = "Guy",
                        Zipcode = "1234AF",
                        DateOfBirth = DateTime.UtcNow.AddMonths(5),
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