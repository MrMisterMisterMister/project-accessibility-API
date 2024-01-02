using System.ComponentModel.DataAnnotations;
namespace Domain{
    public class Preference{
        [Required]
        public int AccountId{get;set;}
        [Required]
        public string Key{get;set;} = null!;
        public string? value{get;set;}

    }
}