using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class Chat : Account{
        [Required]
        public string Title{get;set;} = null!;
        public List<Account?> ?Messages{get;set;}
        public List<Account> ?Recipients{get;set;}
    }
    }