using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class Rolename{
        [Required]
        public string Name{get;set;} = null!;
        
    }
}