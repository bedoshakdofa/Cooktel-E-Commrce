using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cooktel_E_commrece.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[Controller]")]
    public class OrderController:ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        
        public OrderController(ICartRepository cartRepository,
            IOrderRepository orderRepository)       
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }

        [HttpPost("Check-out")]
        public async Task<ActionResult> CreateOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var UserCart = await _cartRepository.GetCartByUserId( Guid.Parse(userId));

            var products = await _cartRepository.GetAllItemsInCart(UserCart.CartId);

            if (products == null) { 
                return NotFound("there is no product in cart");
            }

           var order= _orderRepository.Create(products, Guid.Parse(userId));

            if (await _orderRepository.SaveChangesAsync())
                return StatusCode(201,order);
            return BadRequest("Faild to created order");
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<OrderDto>>> GetAllOrders()
        {
            var userId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var orders = await _orderRepository.GetAllOrder(Guid.Parse(userId));

            if (orders == null)
                return NotFound();
            else 
                return Ok(orders);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult>DeleteOrder(int id)
        {
            await _orderRepository.RemoveOrder(id);

            if (await _orderRepository.SaveChangesAsync())
                return Ok("Deleted Successfully");
            return BadRequest("can't delete the Order");
        }
    }
}

