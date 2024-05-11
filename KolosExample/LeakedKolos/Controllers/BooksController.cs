using LeakedKolos.Exceptions;
using LeakedKolos.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeakedKolos.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBooksService service) : ControllerBase
{
    private readonly IBooksService _service = service;

    [HttpGet("{id:int}/authors")]
    public async Task<IActionResult> GetBookAuthorsAsync(int id)
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
}