using Application.DTOs;
using System.Threading.Tasks;

namespace BSB.src.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(PurchaseDto purchase);
    }
}