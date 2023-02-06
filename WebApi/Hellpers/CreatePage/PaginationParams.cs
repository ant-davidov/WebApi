using System.ComponentModel.DataAnnotations;

namespace WebApi.Hellpers
{
    public class PaginationParams
    {
        [Range(0, int.MaxValue)]
        public int From { get; set; } = 0;
         [Range(1, int.MaxValue)]
        public int Size { get; set; } = 10;        
    }
}
