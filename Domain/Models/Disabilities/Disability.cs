namespace Domain.Models.Disabilities
{
    public class Disability
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<PanelMemberDisability> PanelMembers { get; set; } = new List<PanelMemberDisability>();
    }
}