using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto.RequestDto
{
    public class BookRequestDto
    {
        public string? Title { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
