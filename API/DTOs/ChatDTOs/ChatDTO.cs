using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ChatDTO
    {
       public string User1Id {get;set;} = null!;
       public string user2Id {get;set;} = null!;
       public required string User1Email{get;set;}
       public required string User2Email{get;set;}
    }
}