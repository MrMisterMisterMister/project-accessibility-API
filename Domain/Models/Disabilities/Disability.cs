namespace Domain.Models.Disabilities
{
    public class Disability
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<ExpertDisability> Experts { get; set; } = new List<ExpertDisability>();
    }
}