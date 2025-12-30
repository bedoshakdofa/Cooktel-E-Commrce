using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Data.Models.Enum;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Helper;
using Cooktel_E_commrece.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Cooktel_E_commrece.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentSettings _paySettings;
        private readonly AppDbContext _context;

        public PaymentService(IOptions<PaymentSettings> paySettings, AppDbContext context )
        {
            _paySettings = paySettings.Value;
            _context = context;
        }


        public async Task<string> ProcessPaymentAsync(int orderId, PaymentType type, IEnumerable<CartItemsResponse> cartItems)
        {
            var order = await _context.orders
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Order_ID == orderId);


            if (order == null)
            {
                throw new KeyNotFoundException($"order with ID {orderId} is Not Found");
            }

            if (order.User == null)
            {

                throw new NullReferenceException("this order is not Assigned to user");
            }


            var totalAmount = cartItems.Sum(x => x.Product.Price * x.quantity);
            // Generate a special reference for this transaction
            string transactionRef = Guid.NewGuid().ToString();

            if (type==PaymentType.CashOnDelivery)
            {
                var pay = new Payment
                {
                    Status = PaymentStatus.Pending,
                    totalAmount = totalAmount,
                    TransactionId = transactionRef,
                    Type = PaymentType.CashOnDelivery
                };
                _context.payments.Add(pay);
                order.payment = pay;
                await _context.SaveChangesAsync();
                return "Order success";
            }

            
            var billingData = new
            {
                apartment = "N/A",
                first_name = order.User.FirstName ?? "Guest",
                last_name = order.User.LastName ?? "User",
                street = "N/A",
                building = "N/A",
                phone_number = order.User.PhoneNumber,
                country = "N/A",
                email = order.User.EmailAddress,
                floor = "N/A",
                state = "N/A",
                city = order.User.Address,
            };

            //Create Items Array
            var items = cartItems.Select(x => new
            {
                name = x.Product.ProductName,
                amount = (int)Math.Round(x.unitPrice * 100, MidpointRounding.AwayFromZero),
                description = $"Customer Payment for order no. {order.Order_ID}",
                Quantity = x.quantity
            }).ToArray();


            //Create the Payload for request
            var payload = new
            {
                amount = (int)(totalAmount * 100),
                currency = "EGP",
                payment_methods = new[] { int.Parse(_paySettings.CardIntegrationId) },
                billing_data = billingData,
                items,
                customer = new
                {
                    billingData.first_name,
                    billingData.last_name,
                    billingData.email,
                },
                special_reference = transactionRef,
                expiration = 3600, // 1 hour expiration
                merchant_order_id = transactionRef
            };
            var httpClient = new HttpClient();
            //Prepare the request
            var requestMessage = new HttpRequestMessage(HttpMethod.Post,"https://accept.paymob.com/v1/intention/");
            requestMessage.Headers.Add("Authorization", $"Token {_paySettings.SecretKey}");
            requestMessage.Content = JsonContent.Create(payload);

            //Extracting the Response 
            var response = await httpClient.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Paymob Intention API call failed with status {response.StatusCode}: {responseContent}");
            }

            //Get the Client_secret

            var resultJson = JsonDocument.Parse(responseContent);
            var clientSecret = resultJson.RootElement.GetProperty("client_secret").GetString();


            //Create Payment record in DB
            var payment = new Payment
            {
                Status = PaymentStatus.Pending,
                totalAmount = totalAmount,
                TransactionId=transactionRef,
                Type=PaymentType.CreditCard,
            };
            _context.payments.Add(payment);

            order.payment = payment;

            await _context.SaveChangesAsync();

            string redirectUrl = $"https://accept.paymob.com/unifiedcheckout/?publicKey={_paySettings.PublicKey}&clientSecret={clientSecret}";

            return redirectUrl;
        }


        public async Task UpdateOrSccuess(string specialRef)
        {
            var payment=await _context.payments.Include(o=>o.order_payment).FirstOrDefaultAsync(x=>x.TransactionId==specialRef);

            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with transaction ID {specialRef} not found.");
            }

            payment.Status = PaymentStatus.Success;
            payment.order_payment.OrderStatus=OrderStatus.Paid;


            await _context.SaveChangesAsync();
        }

        public async Task FailOrSuccess(string specialRef)
        {
            var payment = await _context.payments.Include(o => o.order_payment).FirstOrDefaultAsync(x => x.TransactionId == specialRef);

            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with transaction ID {specialRef} not found.");
            }

            payment.Status = PaymentStatus.Failed;
            payment.order_payment.OrderStatus = OrderStatus.Cancelled;
            await _context.SaveChangesAsync();
        }

        public string ComputeHmacSha512(string data)
        {
            var keyBytes=Encoding.UTF8.GetBytes(_paySettings.HMAC);
            var dataBytes=Encoding.UTF8.GetBytes(data);

            using (var hmac=new HMACSHA512(keyBytes)){
                var hash=hmac.ComputeHash(dataBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }


        }

    }
}
