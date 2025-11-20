
namespace Cooktel_E_commrece.Dtos
{
    public class ProductWithReviews
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
      
        public decimal Price { get; set; }

        
        public string color { get; set; }

        public string size { get; set; }

        public string Kind { get; set; }

        public List<string> Image { get; set; } = new List<string>();
        public ICollection<ReviewDto> Reviews { get; set; }
    }
}
