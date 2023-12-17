using System.ComponentModel.DataAnnotations.Schema;

namespace Dto.RequestDto
{
    public class BookRequestDto
    {
        public string? Title { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
