using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
