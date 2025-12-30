using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Cooktel_E_commrece.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class PaymentController:ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentService _paymentService;

        public PaymentController(ICartRepository cartRepository,
            IOrderRepository orderRepository,
            IPaymentService paymentService)
        {
            _cartRepository = cartRepository;
            _paymentService = paymentService;
        }


        [Authorize]
        [HttpPost("payment-request")]
        public async Task<ActionResult> PaymentRequest([FromBody] PaymentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var UserCart = await _cartRepository.GetCartByUserId(Guid.Parse(userId));

            var products = await _cartRepository.GetAllItemsInCart(UserCart.CartId);

            if (products == null)
            {
                return NotFound("there is no product in cart");
            }

            var x = await _paymentService.ProcessPaymentAsync(request.OrderId, request.type, products);

            return Ok(x);
        }


        [HttpGet("callback")]
        public ActionResult CallBackAsync()
        {
            var query = Request.Query;
            string[] fields = new[]
           {
                "amount_cents", "created_at", "currency", "error_occured", "has_parent_transaction",
                "id", "integration_id", "is_3d_secure", "is_auth", "is_capture", "is_refunded",
                "is_standalone_payment", "is_voided", "order", "owner", "pending",
                "source_data.pan", "source_data.sub_type", "source_data.type", "success"
            };

            var stringBuilder = new StringBuilder();

            foreach (var field in fields)
            {
                if (query.TryGetValue(field, out var value))
                {
                    stringBuilder.Append(value);
                }
                else
                {
                    return BadRequest($"Missing expected field: {field}");
                }
            }

            string CalcuatedHmac = _paymentService.ComputeHmacSha512(stringBuilder.ToString());

            if (CalcuatedHmac.Equals(query["hmac"]))
            {
                if (bool.Parse(query["success"]))
                {
                    return StatusCode(201, "Payment success");
                }
                return BadRequest("Payment fail");
            }
            else
            {
                return StatusCode(500, "Secuirty Issue");
            }

        }

        [HttpPost("server-callback")]

        public async Task<ActionResult> ServerCallBack([FromBody] JsonElement payload)
        {
            var query = Request.Query;
            if (!payload.TryGetProperty("obj", out var obj))
                return BadRequest("Missing 'obj' in payload.");

            string[] fields = new[]
           {
                "amount_cents", "created_at", "currency", "error_occured", "has_parent_transaction",
                "id", "integration_id", "is_3d_secure", "is_auth", "is_capture", "is_refunded",
                "is_standalone_payment", "is_voided", "order", "owner", "pending",
                "source_data.pan", "source_data.sub_type", "source_data.type", "success"
           };

            var concatenated = new StringBuilder();
            foreach (var field in fields)
            {
                string[] parts = field.Split('.');
                JsonElement current = obj;
                bool found = true;
                foreach (var part in parts)
                {
                    if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var next))
                        current = next;
                    else
                    {
                        found = false;
                        break;
                    }
                }

                if (!found || current.ValueKind == JsonValueKind.Null)
                {
                    concatenated.Append(""); // Use empty string for missing/null fields
                }
                else if (current.ValueKind == JsonValueKind.True || current.ValueKind == JsonValueKind.False)
                {
                    concatenated.Append(current.GetBoolean() ? "true" : "false"); // Lowercase boolean
                }
                else
                {
                    concatenated.Append(current.ToString());
                }
            }
            string calculatedHmac = _paymentService.ComputeHmacSha512(concatenated.ToString());

            if (!calculatedHmac.Equals(query["hmac"]))
                return StatusCode(500, "Security issues");

            if (obj.TryGetProperty("order", out var order)
                && order.TryGetProperty("merchant_order_id", out var merchantOrderId)
                && merchantOrderId.ValueKind != JsonValueKind.Null)
            {
                bool isSuccess = obj.TryGetProperty("success", out var successElement) && successElement.GetBoolean();

                if (isSuccess)
                {
                    await _paymentService.UpdateOrSccuess(merchantOrderId.ToString());
                }
                else
                {
                    await _paymentService.FailOrSuccess(merchantOrderId.ToString());
                }

            }
            else
            { return StatusCode(500, "something went wrong"); }

            return Ok();

        }

    }
}
