using Application.DTOs;
using Application.Interfaces;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> ProcessPaymentAsync(PurchaseDto purchase) => Task.FromResult(true);
    }
}