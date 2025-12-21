using Cooktel_E_commrece.Data.Models.Enum;
using Cooktel_E_commrece.Dtos;

namespace Cooktel_E_commrece.Interfaces
{
    public interface IPaymentService
    {
        Task<string> ProcessPaymentAsync(int orderId, PaymentType type, IEnumerable<CartItemsResponse> cartItems);
        string ComputeHmacSha512(string data);

        Task FailOrSuccess(string specialRef);

        Task UpdateOrSccuess(string SpecialRef);
    }
}
