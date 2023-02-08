using Microsoft.EntityFrameworkCore;

namespace WebApi.Hellpers.CreatePage
{
    public class PageList<T> : List<T>
    {
        public PageList(IEnumerable<T> items)
        {

            AddRange(items);
        }
        public static async Task<PageList<T>> CreateAsync(IQueryable<T> source, int skip, int pageSize)
        {
            var items = await source.Skip(skip).Take(pageSize).ToListAsync();
            return new PageList<T>(items);
        }
        public static PageList<T> Create(IQueryable<T> source, int skip, int pageSize)
        {
            var items = source.Skip(skip).Take(pageSize).ToList();
            return new PageList<T>(items);
        }
    }
}
