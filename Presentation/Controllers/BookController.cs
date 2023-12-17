using Dto.RequestDto;
using Mapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Contracts;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ILogger<BookController> _logger;

        public BookController(IServiceManager serviceManager, ILogger<BookController> logger)
        {
            _serviceManager = serviceManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _serviceManager.Book.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                _logger.LogInformation($"Attempting to retrieve book with ID: {id}");
                var book = _serviceManager.Book.GetOneBookById(id, false);

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
                _serviceManager.Book.CreateBook(newBook);

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

                _serviceManager.Book.UpdateBook(id, BookMapper.toEntity(bookRequestDto), true);

                _logger.LogInformation($"Book with ID {id} updated successfully.");
                return NoContent();
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
                _serviceManager.Book.DeleteBook(id, false);

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

                var book = _serviceManager.Book.GetOneBookById(id, true);

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

                _serviceManager.Book.UpdateBook(id, BookMapper.toEntity(bookRequestDto), true);

                _logger.LogInformation($"Book with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating book with ID {id}: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
    }
}
