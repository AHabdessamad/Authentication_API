using BusinessLogicLayer;
using BusinessLogicLayer.Results;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _bookService.GetAllBooks();

                if (books == null || !books.Any())
                {
                    return BadRequest("Faild to fetch data") ;
                }

                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server error {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {

                var book = await _bookService.GetBookById(id);

                if (book == null)
                {
                    return BadRequest("Faild to fetch data");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest($"Server error {ex.Message}"); 
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByTitle(string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                {
                    return BadRequest("Invalid Title");
                }

                var MatchedBook = await _bookService.SearchByTitle(title);

                if (MatchedBook == null)
                {
                    return BadRequest("Book  Not Found");
                }

                return Ok(MatchedBook);

            }
            catch (Exception ex)
            {
                return BadRequest($"Server error {ex.Message}");
            }

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response<Book>>> CreateBook([FromBody] BookDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    return Response<Book>.Failure(400, "Invalid book data");
                }

                var createdBook = await _bookService.CreateBook(bookDto);

                if (createdBook == null)
                {
                    return Response<Book>.Failure(400, "Failed to create book");
                }

                return Response<Book>.Success(201, "Book created successfully", createdBook);
                
            }
            catch (Exception ex)
            {
                return Response<Book>.Failure(500, $"Server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    return BadRequest("Invalid book data");
                }

                var updatedBook = await _bookService.UpdateBook(id, bookDto);

                if (updatedBook == null)
                {
                    return BadRequest($"Book with id {id} not found");
                }

                return Ok(true)
                ;
            }
            catch (Exception ex)
            {
                return BadRequest($"Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<bool> DeleteBook(int id)
        {
                var deletionResult = await _bookService.DeleteBook(id);

                if (!deletionResult)
                {
                    return false;
                }

                return true;
            
        }
    }
}