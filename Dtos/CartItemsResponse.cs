using Cooktel_E_commrece.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Dtos
{
    public class CartItemsResponse
    {
        public ProductResponse Product { get; set; }
        public int quantity { get; set; }
        public Decimal unitPrice { get; set; }
    }
}
