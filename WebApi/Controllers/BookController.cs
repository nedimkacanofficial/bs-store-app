using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto.RequestDto;
using WebApi.Mapper;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly RepositoryContext _context;
        private readonly ILogger<BookController> _logger;

        public BookController(RepositoryContext context, ILogger<BookController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve book with ID: {id}");
                var book = _context.Books.FirstOrDefault(b => b.Id == id);

                if (book is null)
                {
                    _logger.LogWarning($"Book with ID {id} not found. Returning 404 NotFound.");
                    return NotFound();
                }

                _logger.LogInformation($"Successfully retrieved book with ID {id}.");
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving book with ID {id}: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] BookRequestDto bookRequestDto)
        {
            try
            {
                if (bookRequestDto is null)
                {
                    _logger.LogWarning("Invalid input data. Returning 400 Bad Request.");
                    return BadRequest();
                }
                var newBook = BookMapper.toEntity(bookRequestDto);
                _context.Books.Add(newBook);
                _context.SaveChanges();
                _logger.LogInformation($"Book created successfully with ID: {newBook.Id}");

                // HTTP 201 Created yanıtını oluştur ve Location başlığını ekleyerek URI'yi belirle
                return CreatedAtAction(nameof(GetOneBook), new { id = newBook.Id }, newBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating a new book: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] BookRequestDto bookRequestDto)
        {
            try
            {
                if (bookRequestDto is null)
                {
                    _logger.LogWarning("Invalid input data. Returning 400 Bad Request.");
                    return BadRequest();
                }

                var book = _context.Books.FirstOrDefault(b => b.Id == id);

                if (book is null)
                {
                    _logger.LogWarning($"Book with ID {id} not found. Returning 404 NotFound.");
                    return NotFound();
                }

                book.Title = bookRequestDto.Title;
                book.Price = bookRequestDto.Price;
                _context.SaveChanges();

                _logger.LogInformation($"Book with ID {id} updated successfully.");
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating book with ID {id}: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _context.Books.FirstOrDefault(b => b.Id == id);

                if (book is null)
                {
                    _logger.LogWarning($"Book with ID {id} not found. Returning 404 NotFound.");
                    return NotFound();
                }

                _context.Books.Remove(book);
                _context.SaveChanges();

                _logger.LogInformation($"Book with ID {id} deleted successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting book with ID {id}: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookRequestDto> patchDoc)
        {
            try
            {
                if (patchDoc is null)
                {
                    _logger.LogWarning("Invalid input data. Returning 400 Bad Request.");
                    return BadRequest();
                }

                var book = _context.Books.FirstOrDefault(b => b.Id == id);

                if (book is null)
                {
                    _logger.LogWarning($"Book with ID {id} not found. Returning 404 NotFound.");
                    return NotFound();
                }

                var bookRequestDto = BookMapper.toDto(book);
                patchDoc.ApplyTo(bookRequestDto, ModelState);

                if (!TryValidateModel(bookRequestDto))
                {
                    _logger.LogWarning("Invalid input data. Returning 400 Bad Request.");
                    return BadRequest(ModelState);
                }

                book.Title = bookRequestDto.Title;
                book.Price = bookRequestDto.Price;
                _context.SaveChanges();

                _logger.LogInformation($"Book with ID {id} updated successfully.");
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating book with ID {id}: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
