using LeakedKolos.Models;
using LeakedKolos.Models.DTO;

namespace LeakedKolos.Repositories;

public interface IBooksRepository
{
    Task<Book?> GetBookByIdAsync(int idBook);

    Task<IEnumerable<Author>?> GetBooksAuthorsByBookIdAsync(int idBook);
    Task<int> AddBookAsync(string title, IEnumerable<int> authorsIdArray);

    Task<Author?> GetAuthorByNameAsync(string firstName, string lastName);
}