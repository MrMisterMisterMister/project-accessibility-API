using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain{
    public class Account{
        [Required]
       public string Email {get;set;} = null!;
       [Required]
       public string Password {get;set;} = null!;
       public string? phoneNumber {get;set;}
       public List<Role>? Roles {get;set;} 
       public List<Preference>? preferences {get;set;}
       public List<Chat>? Chats {get;set;}
    }
}