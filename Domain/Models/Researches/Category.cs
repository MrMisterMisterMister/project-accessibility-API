using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class Category{
        [Required]
        public Guid Id{get;set;}
        [Required]
        public string Name{get;set;} = null!;
         public int ResearchId { get; set; }

        // Navigation property to the Research
        public Research Research { get; set; } = null!;
    }
    }