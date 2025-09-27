namespace BSB.src.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}