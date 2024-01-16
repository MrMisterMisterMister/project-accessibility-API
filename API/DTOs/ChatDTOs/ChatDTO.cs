using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class ChatDTO
    {
       public string User1Id {get;set;} = null!;
       public string user2Id {get;set;} = null!;
       public string Title{get;set;} = null!;
    }
}