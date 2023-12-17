using Dto.RequestDto;
using Entities;

namespace Mapper
{
    public static class BookMapper
    {
        public static Book toEntity(BookRequestDto bookRequestDto)
        {
            Book book = new Book();
            book.Title = bookRequestDto.Title;
            book.Price = bookRequestDto.Price;
            return book;
        }
        public static BookRequestDto toDto(Book book)
        {
            BookRequestDto bookRequestDto = new BookRequestDto();
            bookRequestDto.Title = book.Title;
            bookRequestDto.Price = book.Price;
            return bookRequestDto;
        }
        public static List<Book> toEntityList(List<BookRequestDto> bookRequestDtoList)
        {
            List<Book> bookList = new List<Book>();
            foreach (BookRequestDto bookRequestDto in bookRequestDtoList)
            {
                bookList.Add(toEntity(bookRequestDto));
            }
            return bookList;
        }
        public static List<BookRequestDto> toDtoList(List<Book> bookList)
        {
            List<BookRequestDto> bookRequestDtoList = new List<BookRequestDto>();
            foreach (Book book in bookList)
            {
                bookRequestDtoList.Add(toDto(book));
            }
            return bookRequestDtoList;
        }
    }
}
