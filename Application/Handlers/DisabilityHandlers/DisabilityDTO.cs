namespace Application.DisabilityHandlers
{
    public class DisabilityDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<string> ExpertId { get; set; } = new List<string>();
    }
}