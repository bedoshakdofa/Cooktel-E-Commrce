using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;

namespace Cooktel_E_commrece.Interfaces
{
    public interface ICartRepository
    {
        Task AddItem(Product product, int quantity, int CartId);

        Task<bool> SaveAllChanges();

        Task<Cart> GetCartByUserId(Guid userId);

        void RemoveItem(CartItems cartItem);

        Task<CartItems> GetItemFromCart(int cartId, int productId);

        Task<IEnumerable<CartItemsResponse>>GetAllItemsInCart(int cartId);
    }
}
