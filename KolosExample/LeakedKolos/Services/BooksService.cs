using LeakedKolos.Exceptions;
using LeakedKolos.Models.DTO;
using LeakedKolos.Repositories;

namespace LeakedKolos.Services;

public class BooksService(IBooksRepository repository) : IBooksService
{
    private readonly IBooksRepository _repository = repository;


    public async Task<object> GetBooksInfoByIdAsync(int idBook)
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

        return new
        {
            id = book.IdBook,
            title = book.Title,
            authors = authorsDto
        };
    }

    public async Task<object> AddBookWithAuthors(BookInfoDto bookInfoDto)
    {
        var authorsIds = new List<int>();
        foreach (var authorDto in bookInfoDto.Authors)
        {
            var temp = await _repository.GetAuthorByNameAsync(authorDto.FirstName, authorDto.LastName);
            if (temp is null)
            {
                throw new AuthorNotFoundException($"Author {authorDto.FirstName} " +
                                                  $"{authorDto.LastName} does not exist.");
            }
            authorsIds.Add(temp.IdAuthor);
        }

        var insertedId = await _repository.AddBookAsync(bookInfoDto.Title, authorsIds);
        if (insertedId == -1)
        {
            throw new Exception("Internal server error occurred");
        }

        return new
        {
            id = insertedId,
            title = bookInfoDto.Title,
            authors = bookInfoDto.Authors
        };
    }
}