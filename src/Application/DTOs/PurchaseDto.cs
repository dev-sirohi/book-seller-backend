namespace Application.DTOs
{
    public class PurchaseDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime PurchasedAt { get; set; }
        public decimal Amount { get; set; }
    }
}