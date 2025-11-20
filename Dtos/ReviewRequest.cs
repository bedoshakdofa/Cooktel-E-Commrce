using Cooktel_E_commrece.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cooktel_E_commrece.Dtos
{
    public class ReviewRequest
    {
        [Required]
        public string Comment { get; set; }
        [Required]
        public int Product_ID { get; set; }
    }
}
