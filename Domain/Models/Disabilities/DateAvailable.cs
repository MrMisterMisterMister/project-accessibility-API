using System.ComponentModel.DataAnnotations;

namespace Domain{
    public class DateAvailable{
        [Required]
        public int Year{get;set;}
        [Required]
        public int Month{get;set;}
        [Required]
        public int Day{get;set;}
        [Required]
        public int BeginTime{get;set;}
        [Required]
        public int EndTime{get;set;}
    }
}