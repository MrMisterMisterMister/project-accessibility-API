using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class Category{
        [Required]
        public Guid Id{get;set;}
        [Required]
        public string Name{get;set;} = null!;
    }
    }