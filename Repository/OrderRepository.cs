using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Data.Models.Enum;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public OrderDto Create(IEnumerable<CartItemsResponse> cartItems, Guid userId)
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
            return _mapper.Map<OrderDto>(newOrder);
        }


        public async Task<IReadOnlyCollection<OrderDto>> GetAllOrder(Guid Userid)
        {
            return await _context.orders
                .Where(x=>x.OrderStatus==OrderStatus.pending&&x.UserId==Userid)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task RemoveOrder(int id)
        {
            var order = await _context.orders.FirstOrDefaultAsync(x => x.Order_ID == id);

            if (order == null)
                throw new ArgumentException($"there is not order with {id}");

            _context.orders.Remove(order);  
        }


        public async Task<bool> SaveChangesAsync()
        {
            if(await  _context.SaveChangesAsync()>0) return true; return false;
        }
    }
}
