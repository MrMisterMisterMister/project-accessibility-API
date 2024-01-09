using System.ComponentModel.DataAnnotations;
using Domain;

public class PanelMember : User
{
    [Required]
    public int Guardian { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string? Zipcode { get; set; }
    public DateTime DateOfBirth { get; set; }

}