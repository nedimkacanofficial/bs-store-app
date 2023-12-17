using Entities;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks(bool trackChanges);
        Book GetOneBookById(int id, bool trackChanges);
        Book CreateBook(Book book);
        void UpdateBook(int id, Book book, bool trackChanges);
        void DeleteBook(int id, bool trackChanges);
    }
}
