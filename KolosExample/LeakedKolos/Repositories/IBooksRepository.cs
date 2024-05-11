using LeakedKolos.Models;

namespace LeakedKolos.Repositories;

public interface IBooksRepository
{
    Task<Book?> GetBookByIdAsync(int idBook);

    Task<IEnumerable<Author>?> GetBooksAuthorsByBookIdAsync(int idBook);
}