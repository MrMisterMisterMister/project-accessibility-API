using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Research
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string Title { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public DateTime Date { get; set; }
        
        public bool isOnline { get; set; }
        
        public int Reward { get; set; }

        public List<PanelMember> Participants { get; set; } = new List<PanelMember>();

        public Company Organizer { get; set; } = null!;
     [Required]
        public List<ResearchCategory> ResearchCategories { get; set; } = null!; 
    }
}
