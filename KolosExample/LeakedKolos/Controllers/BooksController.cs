using LeakedKolos.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeakedKolos.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBooksService service) : ControllerBase
{
    private IBooksService _service = service;

    [HttpGet("{id:int}/authors")]
    public IActionResult GetBookAuthorsAsync(int id)
    {
        return Ok();
    }
}