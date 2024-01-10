using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SeedData
{
    public class ResearchBase
    {
        public static async Task SeedResearches(DataContext context)
        {
            var panelMembers = await context.Users.OfType<PanelMember>().ToListAsync();
            var companies = await context.Users.OfType<Company>().ToListAsync();

            if (!panelMembers.Any() || !companies.Any())
            {
                return;
            }

            if (!context.Researches.Any())
            {
                var researches = new List<Research> {
                    new Research
                    {
                        Title = "Research 1",
                        Description = "Description for Research 1",
                        Date = DateTime.UtcNow.AddMonths(1),
                        isOnline = true,
                        Reward = 50,
                        Organizer = companies[0],
                        Participants = panelMembers
                        .OrderBy(_ => Guid.NewGuid()) .Take(2).Select(pm => new Participant {
                            PanelMember = pm,
                            Status = GetRandomStatus(),
                        })
                        .ToList(),
                        Categories = new List<Category>
                        {
                            new Category { Name = "Banana" },
                            new Category { Name = "Apple" }
                        }
                    },
                    new Research
                    {
                        Title = "Research 2",
                        Description = "Description for Research 2",
                        Date = DateTime.UtcNow.AddMonths(2),
                        isOnline = false,
                        Reward = 30,
                        Organizer = companies[1],
                        Participants = panelMembers
                        .OrderBy(_ => Guid.NewGuid()) .Take(2).Select(pm => new Participant {
                            PanelMember = pm,
                            Status = GetRandomStatus(),
                        })
                        .ToList(),
                        Categories = new List<Category>
                        {
                            new Category { Name = "Horse" },
                            new Category { Name = "Cow" }
                        }
                    },
                };

                context.Researches.AddRange(researches);
                await context.SaveChangesAsync();
            }
        }

        private static string GetRandomStatus()
        {
            var status = new[] { "Online", "Offline", "Pending", "Approved", "Rejected" };
            var random = new Random();
            return status[random.Next(status.Length)];
        }
    }
}