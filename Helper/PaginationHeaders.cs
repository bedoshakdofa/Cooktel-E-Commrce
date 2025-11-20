namespace Cooktel_E_commrece.Helper
{
    public class PaginationHeaders
    {
        public PaginationHeaders(int currantPage, int itemPerPage, int totalPages, int totalItems)
        {
            CurrantPage = currantPage;
            ItemPerPage = itemPerPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }

        public int CurrantPage { get; set; }

        public int ItemPerPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }
    }
}
