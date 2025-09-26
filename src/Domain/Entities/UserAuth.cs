namespace BSB.src.Domain.Entities
{
    public class UserAuth
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
