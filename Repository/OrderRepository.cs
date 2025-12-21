using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Data.Models.Enum;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;

namespace Cooktel_E_commrece.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(AppDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Create(IEnumerable<CartItemsResponse> cartItems, Guid userId)
        {
            var newOrder = new Orders
            {
                OrderStatus = OrderStatus.pending,
                UserId = userId
            };
            
            _context.orders.Add(newOrder);


            foreach (var item in  cartItems)
            {
                _context.orderItems.Add(new OrderItems
                {
                    orders=newOrder,
                    Product_ID=item.Product.Id,
                    quantity=item.quantity,
                });
            }
        }



        public async Task<bool> SaveChangesAsync()
        {
            if(await  _context.SaveChangesAsync()>0) return true; return false;
        }
    }
}
