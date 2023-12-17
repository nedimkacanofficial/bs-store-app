using Entities;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;

        public BookManager(IRepositoryManager manager, ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public Book CreateBook(Book book)
        {
            if (book is null)
            {
                _logger.LogInfo("Book object sent from client is null.");
                throw new ArgumentNullException(nameof(book));
            }
            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteBook(int id, bool trackChanges)
        {
            // Check if book exists
            var book = _manager.Book.GetOneBookById(id, trackChanges);
            if (book is null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException(nameof(book));
            }
            _manager.Book.DeleteOneBook(book);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book.GetAllBooks(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            return _manager.Book.GetOneBookById(id, trackChanges);
        }

        public void UpdateBook(int id, Book book, bool trackChanges)
        {
            if (book is null)
            {
                _logger.LogInfo("Book object sent from client is null.");
                throw new ArgumentNullException(nameof(book));
            }
            // Check if book exists
            var bookEntity = _manager.Book.GetOneBookById(id, trackChanges);
            if (bookEntity is null)
            {
                _logger.LogInfo($"Book with id: {id} doesn't exist in the database.");
                throw new ArgumentNullException(nameof(bookEntity));
            }

            // Update book
            bookEntity.Title = book.Title;
            bookEntity.Price = book.Price   ;
            _manager.Book.UpdateOneBook(bookEntity);
            _manager.Save();
        }
    }
}
