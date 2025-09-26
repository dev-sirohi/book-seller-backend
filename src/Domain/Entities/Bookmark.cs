namespace BSB.src.Domain.Entities
{
    public class Bookmark
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public int PageNumber { get; set; }
    }
}