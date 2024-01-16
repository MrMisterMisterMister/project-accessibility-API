using Domain;
using Domain.Models.Disabilities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.SeedData
{
    public class DisabilitiesBase
    {
        public static async Task SeedDisabilites(DataContext context)
        {
            var panelMembers = await context.Users.OfType<PanelMember>().ToListAsync();

            if (!panelMembers.Any())
                return;

            if (!context.Disabilities.Any())
            {
                var disabilites = new List<Disability>
                {
                    new Disability {
                        Name = "John Cena",
                        Description = "U Can't C Me!",
                        Experts = new List<ExpertDisability>()
                        {
                            new ExpertDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability {
                        Name = "Blindness",
                        Description = "This person cannot see.",
                        Experts = new List<ExpertDisability>()
                        {
                            new ExpertDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability {
                        Name = "Deaf",
                        Description = "This person cannot hear.",
                        Experts = new List<ExpertDisability>()
                        {
                            new ExpertDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability {
                        Name = "Mute",
                        Description = "This person cannot speak.",
                        Experts = new List<ExpertDisability>()
                        {
                            new ExpertDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "Parkinson's Disease",
                        Description = "This person with Parkinson's disease may experience symptoms like tremors, " +
                                    "stiffness, and difficulty with movement due to a loss of dopamine-producing cells " +
                                    "in the brain. Additionally, cognitive issues and mood changes can occur. " +
                                    "Although there is no cure, treatments are aimed at alleviating symptoms and " +
                                    "improving overall well-being. Regular medical monitoring and support are crucial " +
                                    "for managing the challenges associated with Parkinson's disease.",
                        Experts = new List<ExpertDisability>
                        {
                            new ExpertDisability
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                };

                await context.Disabilities.AddRangeAsync(disabilites);
                await context.SaveChangesAsync();
            }
        }
    }
}