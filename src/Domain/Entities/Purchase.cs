namespace Domain.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime PurchasedAt { get; set; }
        public decimal Amount { get; set; }
    }
}