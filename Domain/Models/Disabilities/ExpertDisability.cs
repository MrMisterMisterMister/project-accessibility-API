namespace Domain.Models.Disabilities
{
    public class ExpertDisability
    {
        public int DisabilityId { get; set; }
        public Disability Disability { get; set; } = null!;
        public string PanelMemberId { get; set; } = null!;
        public PanelMember PanelMember { get; set; } = null!;
    }
}