using Cooktel_E_commrece.Data.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Dtos
{
    public class OrderDto
    {
        public int Order_ID { get; set; }

    
        public OrderStatus OrderStatus { get; set; }
      
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
