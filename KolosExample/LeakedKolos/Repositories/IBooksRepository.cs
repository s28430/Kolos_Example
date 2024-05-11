using LeakedKolos.Models;
using LeakedKolos.Models.DTO;

namespace LeakedKolos.Repositories;

public interface IBooksRepository
{
    Task<Book?> GetBookByIdAsync(int idBook);
}