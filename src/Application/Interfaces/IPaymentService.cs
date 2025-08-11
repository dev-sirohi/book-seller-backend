using Application.DTOs;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPaymentAsync(PurchaseDto purchase);
    }
}