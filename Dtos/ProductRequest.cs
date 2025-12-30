using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cooktel_E_commrece.Dtos
{
    public class ProductRequest
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string color { get; set; }

        [Required]
        [MaxLength(25)]
        public string size { get; set; }

        [Required]
        [MaxLength(50)]
        public string Kind { get; set; }

        [Required]
        public IFormFile Image { get; set; }
         
        [Required]
        public int ProductStock { get; set; }

        [Required]
        public int SubCategoryID { get; set; }

    }
}
