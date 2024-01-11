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
                        Type = "Online",
                        Category = "Category 1",
                        Reward = 20.5,
                        Organizer = companies[0],
                        Participants = panelMembers
                        .OrderBy(_ => Guid.NewGuid()).Take(3).Select(pm => new Participant {
                            PanelMember = pm,
                            DateJoined = DateTime.Today,
                            Status = "Active",
                        })
                        .ToList(),
                    },
                    new Research
                    {
                        Title = "Research 2",
                        Description = "Description for Research 2",
                        Date = DateTime.UtcNow.AddMonths(2),
                        Type = "Online",
                        Category = "Category 2",
                        Reward = 10.20,
                        Organizer = companies[1],
                        Participants = panelMembers
                        .OrderBy(_ => Guid.NewGuid()).Take(1).Select(pm => new Participant {
                            PanelMember = pm,
                            DateJoined = DateTime.Today,
                            Status = "Active",
                        })
                        .ToList(),
                    },
                    new Research
                    {
                        Title = "Research 3",
                        Description = "Description for Research 3",
                        Date = DateTime.UtcNow.AddMonths(3),
                        Type = "Online",
                        Category = "Category 3",
                        Reward = 53.24,
                        Organizer = companies[2],
                        Participants = panelMembers
                        .OrderBy(_ => Guid.NewGuid()).Take(1).Select(pm => new Participant {
                            PanelMember = pm,
                            DateJoined = DateTime.Today,
                            Status = "Active",
                        })
                        .ToList(),
                    },
                    new Research
                    {
                        Title = "Research 4",
                        Description = "Description for Research 4",
                        Date = DateTime.UtcNow.AddMonths(1),
                        Type = "Interview",
                        Category = "Category 4",
                        Reward = 44.44,
                        Organizer = companies[3],
                        Participants = panelMembers
                        .OrderBy(_ => Guid.NewGuid()).Take(2).Select(pm => new Participant {
                            PanelMember = pm,
                            DateJoined = DateTime.Today,
                            Status = "Active",
                        })
                        .ToList(),
                    },
                    new Research
                    {
                        Title = "Research 5",
                        Description = "Description for Research 5",
                        Date = DateTime.UtcNow.AddMonths(1),
                        Type = "Case Study",
                        Category = "Category 5",
                        Reward = 91.98,
                        Organizer = companies[4],
                        Participants = panelMembers
                        .OrderBy(_ => Guid.NewGuid()).Take(1).Select(pm => new Participant {
                            PanelMember = pm,
                            DateJoined = DateTime.Today,
                            Status = "Active",
                        })
                        .ToList(),
                    },
                };

                context.Researches.AddRange(researches);
                await context.SaveChangesAsync();
            }
        }
    }
}