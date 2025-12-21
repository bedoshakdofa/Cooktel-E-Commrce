using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper; 
        public CartRepository(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddItem(Product product, int quantity, int CartId)
        {
            var CartItem = await _context.cartItems.FirstOrDefaultAsync(x => x.CartId == CartId && x.ProductId == product.Id);

            if (CartItem == null)
            {
                var newCartItem = new CartItems
                {
                    ProductId = product.Id,
                    unitPrice = product.Price,
                    CartId = CartId,
                    quantity = quantity
                };

                _context.cartItems.Add(newCartItem);
            }
            else
            {
                CartItem.quantity += quantity;
            }
        }
        public async Task<IEnumerable<CartItemsResponse>> GetAllItemsInCart(int cartId)
        {
            var query= _context.cartItems.AsNoTracking().AsQueryable();

            var items= await query.Include(ci => ci.Product)
                .Where(ci => ci.CartId == cartId)
                .ProjectTo<CartItemsResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return items;
        }

        public async Task<Cart> GetCartByUserId(Guid userId)
        {
            var cart = await _context.carts.Include(x=>x.cartItems).FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {

                cart = new Cart { UserId = userId };

                _context.carts.Add(cart);

                await _context.SaveChangesAsync();
            }
            return cart;
        }

        public async Task<CartItems> GetItemFromCart(int cartId, int productId)
        {
            return await _context.cartItems
                .FirstOrDefaultAsync(cr => cr.CartId == cartId && cr.ProductId == productId);
        }

        public void RemoveItem(CartItems cartItem)
        {
            _context.cartItems.Remove(cartItem);
        }

        public async Task<bool> SaveAllChanges()
        {
            if (await _context.SaveChangesAsync()>0)
                return true;
            return false;
        }

       
    }
}
