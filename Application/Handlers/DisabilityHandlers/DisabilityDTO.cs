namespace Application.DisabilityHandlers
{
    public class DisabilityDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<string> PanelMemberId { get; set; } = new List<string>();
    }
}