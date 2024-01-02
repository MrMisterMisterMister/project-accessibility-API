using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class Category{
        [Required]
        public string Name{get;set;} = null!;
    }
    }