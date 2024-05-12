using LeakedKolos.Exceptions;
using LeakedKolos.Models.DTO;
using LeakedKolos.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeakedKolos.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBooksService service) : ControllerBase
{
    private readonly IBooksService _service = service;

    [HttpGet("{id:int}/authors")]
    public async Task<IActionResult> GetBookInfo(int id)
    {
        try
        {
            return Ok(await _service.GetBooksInfoByIdAsync(id));
        }
        catch (BookNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(BookInfoDto bookInfoDto)
    {
        try
        {
            return StatusCode(201, await _service.AddBookWithAuthors(bookInfoDto));
        }
        catch (AuthorNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}