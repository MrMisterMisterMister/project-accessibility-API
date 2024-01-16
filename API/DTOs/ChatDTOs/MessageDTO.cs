using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class MessageDTO
    {
        public int ChatId{get;set;}
       public required string SenderId{get;set;}
       public required string Content {get;set;}
    }
}