using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Extenstions;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cooktel_E_commrece.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    [Authorize]
    public class CartController:ControllerBase
    {
        private readonly IProductRepository _productRepository;

        private readonly ICartRepository _cartRepository;

        public CartController(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        [HttpPost("Add-Item")]
        public async Task<ActionResult> AddToCart(AddCartItemDto addCartItem)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _cartRepository.GetCartByUserId(Guid.Parse(userId));

            var product = await _productRepository.GetById(addCartItem.ProductId);

            if (product == null)
                return NotFound("Product not found");

            if (product.ProductStock < addCartItem.Quantity)
                return BadRequest("Not enough stock");

            await _cartRepository.AddItem(product, addCartItem.Quantity, cart.CartId);

            if (await _cartRepository.SaveAllChanges())
                return Ok("Added to cart");
            return BadRequest("Failed to add to cart");
        }

        [HttpGet("Get-Items")]
        public async Task<ActionResult<IEnumerable<CartItems>>>GetItemsInCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _cartRepository.GetCartByUserId(Guid.Parse(userId));

            var items = await _cartRepository.GetAllItemsInCart(cart.CartId);

            foreach(var item in items)
            {
                HttpContext.AddImageLink(item.Product.Image);
            }

            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveItemInCart(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _cartRepository.GetCartByUserId(Guid.Parse(userId));

            if (cart == null) return NotFound("no cart found");

            var item = await _cartRepository.GetItemFromCart(cart.CartId, id);

            _cartRepository.RemoveItem(item);

            if (await _cartRepository.SaveAllChanges())
                return Ok("Removed From Cart");
            return BadRequest("Failed to remove from cart");

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemInCart([FromRoute]int id, [FromBody] string answer)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _cartRepository.GetCartByUserId(Guid.Parse(userId));

            if (cart == null) return NotFound("no cart found");

            var item = await _cartRepository.GetItemFromCart(cart.CartId, id);

            if (item == null) return NotFound("this item is not in cart");

            if (answer == "increase")
            {
                item.quantity++;
            }
            else if (answer == "decrease")
            {
                item.quantity--;
            }

            if (await _cartRepository.SaveAllChanges())
                return Ok("Removed From Cart");
            return BadRequest("Failed to remove from cart");

        }

    }
}
