namespace Cooktel_E_commrece.Helper
{
    public class FilterParams:UserParams
    {
        public string OrderByPrice { get; set; }

        public string color { get; set; }

        public string size { get; set; }

        public int categoryId { get; set; }
    }
}
