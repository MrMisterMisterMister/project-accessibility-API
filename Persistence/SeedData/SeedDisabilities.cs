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
                        Name = "Clodsire's Disease",
                        Description = "Clodsire's Disease is a unique condition that goes beyond conventional " +
                        "understanding. Those diagnosed experience overwhelming adoration and love for Clodsire, " +
                        "creating an unbreakable bond. Symptoms include perpetual warmth, admiration, and uncontrollable " +
                        "smiling at the thought of Clodsire. There's no physical harm; instead, it fosters deep, " +
                        "unconditional love.Treatment isn't pursued, embracing it as a positive aspect of emotional " +
                        "well - being. Supportive communities celebrate and share experiences, recognizing Clodsire's " +
                        "profound impact. In summary, Clodsire's Disease brings unparalleled love and joy, celebrated " +
                        "as a unique aspect of the human experience. Clodsire my beloved <3",
                        PanelMembers = panelMembers.Select(panelMember => new PanelMemberDisability
                        {
                            PanelMember = panelMember
                        }).ToList()
                    },
                    new Disability
                    {
                        Name = "John Cena",
                        Description = "U Can't C Me!",
                        PanelMembers = new List<PanelMemberDisability>()
                        {
                            new PanelMemberDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "Visual impairment",
                        Description = "This may include conditions like low vision or blindness, where " +
                            "individuals may have difficulty seeing or may rely on alternative methods for " +
                            "accessing information.",
                        PanelMembers = new List<PanelMemberDisability>()
                        {
                            new PanelMemberDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "Hearing impairment",
                        Description = "This refers to difficulty hearing or deafness, where individuals " +
                            "may use sign language, lip-reading, or hearing aids to communicate.",
                        PanelMembers = new List<PanelMemberDisability>()
                        {
                            new PanelMemberDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "Mobility impairment",
                        Description = "Individuals with mobility impairments may experience difficulties " +
                            "with physical movement, such as walking or using fine motor skills.",
                        PanelMembers = new List<PanelMemberDisability>()
                        {
                            new PanelMemberDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "Cognitive impairment",
                        Description = "This includes difficulties with thinking, learning, or memory. " +
                            "Conditions like dyslexia or cognitive disorders may fall into this category.",
                        PanelMembers = new List<PanelMemberDisability>()
                        {
                            new PanelMemberDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "Autism",
                        Description = "Neurodevelopmental condition with distinct social communication styles and sensory " +
                            "sensitivities. Embracing neurodiversity is crucial for support.",
                        PanelMembers = new List<PanelMemberDisability>()
                        {
                            new PanelMemberDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "ADHD",
                        Description = "Attention-Deficit/Hyperactivity Disorder characterized by inattention, " +
                            "hyperactivity, and impulsivity. Strategies include structured routines and personalized " +
                            "learning approaches.",
                        PanelMembers = new List<PanelMemberDisability>()
                        {
                            new PanelMemberDisability()
                            {
                                PanelMember = panelMembers[new Random().Next(0, 5)]
                            }
                        }
                    },
                    new Disability
                    {
                        Name = "Parkinson's Disease",
                        Description = "This person with Parkinson's disease may experience symptoms like tremors " +
                                    "stiffness, and difficulty with movement due to a loss of dopamine-producing cells " +
                                    "in the brain. Additionally, cognitive issues and mood changes can occur. " +
                                    "Although there is no cure, treatments are aimed at alleviating symptoms and " +
                                    "improving overall well-being. Regular medical monitoring and support are crucial " +
                                    "for managing the challenges associated with Parkinson's disease.",
                        PanelMembers = new List<PanelMemberDisability>
                        {
                            new PanelMemberDisability
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