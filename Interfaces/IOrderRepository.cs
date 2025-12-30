using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Data.Models.Enum;
using Cooktel_E_commrece.Dtos;

namespace Cooktel_E_commrece.Interfaces
{
    public interface IOrderRepository
    {
        OrderDto Create(IEnumerable< CartItemsResponse >cartItems, Guid userID);

        Task<IReadOnlyCollection<OrderDto>>GetAllOrder(Guid Userid);

        Task RemoveOrder(int id);
        Task<bool> SaveChangesAsync();
    }
}
