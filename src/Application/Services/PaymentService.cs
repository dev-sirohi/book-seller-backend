using BSB.src.Application.Interfaces;
using System.Threading.Tasks;

namespace BSB.src.Application.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> ProcessPaymentAsync(PurchaseDto purchase) => Task.FromResult(true);
    }
}