using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Domain{
    public class Research{
        [Required]
        public Guid Id{get;set;}
        [Required]
        public string Title{get;set;} = null!;
        public string? Description{get;set;}
        public DateTime Date{get;set;}
        public bool isOnline {get;set;}
        public int Reward{get;set;}
        public List<PanelMember?>? Participants {get;set;}
        public Company ?Organizer {get;set;}
        [Required]
        public List<Category> Categories{get;set;} = null!;

        public static implicit operator Research(Guid v)
        {
            throw new NotImplementedException();
        }
    }
}