namespace BSB.src.Domain.Entities
{
    public class UserAuthLink
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string Link { get; set; } = string.Empty;
        public int LinkType { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
