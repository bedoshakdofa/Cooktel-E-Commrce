using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Helper
{
    public class PagedList<T>:List<T>
    {
        public PagedList(IEnumerable<T> items,int PageNumber, int pageSize, int Count)
        {
            CurrantPage = PageNumber;
            TotalPages = (int)Math.Ceiling(Count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = Count;
            AddRange(items);
        }

        public int CurrantPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }


        public static async Task<PagedList<T>>CreateAsync(IQueryable<T> Source, int PageNumber, int PageSize)
        {
            var count =await Source.CountAsync();

            var items = await Source.Skip((PageNumber - 1)*PageSize).Take(PageSize).ToListAsync();

            return new PagedList<T>(items,PageNumber, PageSize, count);
        }
    }




}
