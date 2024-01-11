namespace Domain
{
    public class ResearchParticipant
    {
        public int ResearchId { get; set; }
        public Research Research { get; set; } = null!;
        public string PanelMemberId { get; set; } = null!;
        public PanelMember PanelMember { get; set; } = null!;
        public DateTime DateJoined { get; set; }
    }
}
