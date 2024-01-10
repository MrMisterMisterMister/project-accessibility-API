using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Research
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool isOnline { get; set; }
        public int Reward { get; set; }
         [Required]
        public string OrganizerId { get; set; } = null!;
        public Company Organizer { get; set; } = null!;

        public List<Participant> Participants { get; set; } = new List<Participant>();
        public List<Category> Categories { get; set; } = null!;
}
}