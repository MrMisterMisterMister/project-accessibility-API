namespace Domain
{
    public class ResearchCategory
    {
        public Guid ResearchId { get; set; }
        public Research Research { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
