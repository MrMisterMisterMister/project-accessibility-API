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
                        Title = "My dog has Schizophrenia and Color blindness",
                        Description = "In this workshop we will help you better understand your dog who suffers from schizophrenia and color blindness.",
                        Date = DateTime.UtcNow.AddDays(6),
                        Type = "Workshop",
                        Category = "Color blindness, Schizophrenia",
                        Reward = 69.9,
                        Organizer = companies[0],
                        Participants = new List<ResearchParticipant>()
                        {
                            new ResearchParticipant() {
                                PanelMember = panelMembers[new Random().Next(0, 5)],
                                DateJoined = DateTime.UtcNow.AddYears(new Random().Next(0, 100))
                            }
                        }
                    },
                    new Research
                    {
                        Title = "Questionnaire for Visually impairment",
                        Description = "Fill in the questionnaire regarding your struggles being visually impaired.",
                        Date = DateTime.UtcNow.AddDays(21),
                        Type = "Online",
                        Category = "Visual impairment",
                        Reward = 15.99,
                        Organizer = companies[1],
                        Participants = new List<ResearchParticipant>()
                        {
                            new ResearchParticipant() {
                                PanelMember = panelMembers[new Random().Next(0, 5)],
                                DateJoined = DateTime.UtcNow.AddYears(new Random().Next(0, 100))
                            }
                        }
                    },
                    new Research
                    {
                        Title = "Mushrooms are growing on my hands",
                        Description = "Have you shaken Joe Biden's hands and not washed them for 6 years?",
                        Date = DateTime.UtcNow.AddDays(52),
                        Type = "Online",
                        Category = "Mold, Joe Biden, Armless",
                        Reward = 53.24,
                        Organizer = companies[2],
                        Participants = new List<ResearchParticipant>()
                        {
                            new ResearchParticipant() {
                                PanelMember = panelMembers[new Random().Next(0, 5)],
                                DateJoined = DateTime.UtcNow.AddYears(new Random().Next(0, 100))
                            }
                        }
                    },
                    new Research
                    {
                        Title = "Ore no Myrtle ga Konna ni Kawaii Wake ga Nai",
                        Description = "Do you wish to be interviewed by Myrtle, the cutest Durin ever? You are in luck, sign up quickly!",
                        Date = DateTime.UtcNow.AddDays(4),
                        Type = "Interview",
                        Category = "Myrtle, Durin, Flagbearer, Cute",
                        Reward = 9.95,
                        Organizer = companies[3],
                        Participants = new List<ResearchParticipant>()
                        {
                            new ResearchParticipant() {
                                PanelMember = panelMembers[new Random().Next(0, 5)],
                                DateJoined = DateTime.UtcNow.AddYears(new Random().Next(0, 100))
                            }
                        }
                    },
                    new Research
                    {
                        Title = "John Cena is invisable",
                        Description = "Has it ever happened that you can't see John Cena? Well, we can't see him either.",
                        Date = DateTime.UtcNow.AddDays(34),
                        Type = "Case Study",
                        Category = "John Cena, I fell when I was a baby",
                        Reward = 100.05,
                        Organizer = companies[4],
                        Participants = new List<ResearchParticipant>()
                        {
                            new ResearchParticipant() {
                                PanelMember = panelMembers[new Random().Next(0, 5)],
                                DateJoined = DateTime.UtcNow.AddYears(new Random().Next(0, 100))
                            }
                        }
                    },
                    new Research
                    {
                        Title = "My stepsister can't be disabled",
                        Description = "This description will make you very sad, so don't read it.",
                        Date = DateTime.UtcNow.AddDays(12),
                        Type = "Online",
                        Category = "Girls only, Cognitive dissonance",
                        Reward = 100.05,
                        Organizer = companies[2],
                        Participants = new List<ResearchParticipant>()
                        {
                            new ResearchParticipant() {
                                PanelMember = panelMembers[new Random().Next(0, 5)],
                                DateJoined = DateTime.UtcNow.AddYears(new Random().Next(0, 100))
                            }
                        }
                    },
                };

                await context.Researches.AddRangeAsync(researches);
                await context.SaveChangesAsync();
            }
        }
    }
}