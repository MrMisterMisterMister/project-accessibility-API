using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class Message : Account{
        [Required]
        public Account Sender{get;set;} = null!;
        [Required]
        public string Content{get;set;} = null!;
        public DateTime Timestamp{get;set;}
        public DateTimeOffset Timezone{get;set;}
    }
    }