using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class Disability{
        [Required]
        public string Name{get;set;} = null!;
        public string? Description{get;set;}
    }
}