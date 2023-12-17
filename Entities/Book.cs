using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
