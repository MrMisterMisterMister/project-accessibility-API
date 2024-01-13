using System.ComponentModel.DataAnnotations;
using Domain;

namespace API.DTOs
{
    public class DeleteParticipantDTO
    {
       public int researchId{get;set;}
       public string participantid{get;set;} = null!;
    }
}