using LeakedKolos.Exceptions;
using LeakedKolos.Models.DTO;
using LeakedKolos.Repositories;

namespace LeakedKolos.Services;

public class BooksService(IBooksRepository repository) : IBooksService
{
    private readonly IBooksRepository _repository = repository;


    public async Task<ResponseBookDto> GetBooksInfoByIdAsync(int idBook)
    {
        var book = await _repository.GetBookByIdAsync(idBook);
        if (book is null)
        {
            throw new BookNotFoundException($"Book with id {idBook} does not exist.");
        }

        var authors = await _repository.GetBooksAuthorsByBookIdAsync(idBook);

        // zakladam ze jest mozliwe ze isntije ksiazka bez autorow
        // gdyby tak nie bylo, trzeba by bylo rzucic wyjatek w przypadku gdy authors is null
        var authorsDto = authors is null 
            ? Array.Empty<AuthorDto>() 
            : authors.Select(author => new AuthorDto(author.FirstName, author.LastName));
        
        return new ResponseBookDto(book.IdBook, book.Title, authorsDto);
    }
}