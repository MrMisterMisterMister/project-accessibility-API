using Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SeedData{
    public class ResearchBase{
        public static async Task SeedResearches(DataContext context){
            var panelMembers = await context.Users.OfType<PanelMember>().ToListAsync();
            var companies = await context.Users.OfType<Company>().ToListAsync();

            if (!panelMembers.Any() || !companies.Any())
            {
                return;
            }

            if (!context.Researches.Any())
            {
                var researches = new List<Research>
                {
                    new Research
                    {
                        Title = "Research 1",
                        Description = "Description for Research 1",
                        Date = DateTime.UtcNow.AddMonths(1),
                        isOnline = true,
                        Reward = 50,
                        Organizer = companies[0],
                        Participants = panelMembers.Take(2).ToList(), 
                        ResearchCategories = new List<ResearchCategory>
                        {
                            new ResearchCategory { Category = new Category { Name = "Category1" } },
                            new ResearchCategory { Category = new Category { Name = "Category2" } }
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
                        Participants = panelMembers.Skip(2).Take(2).ToList(), 
                        ResearchCategories = new List<ResearchCategory>
                        {
                            new ResearchCategory { Category = new Category { Name = "Category3" } },
                            new ResearchCategory { Category = new Category { Name = "Category4" } }
                        }
                    },
                };

                context.Researches.AddRange(researches);
                await context.SaveChangesAsync();
            }
        }
    }
}
