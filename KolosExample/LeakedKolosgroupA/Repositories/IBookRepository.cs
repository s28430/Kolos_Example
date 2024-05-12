using LeakedKolosgroupA.Models;

namespace LeakedKolosgroupA.Repositories;

public interface IBookRepository
{
    public Task<IEnumerable<Author>> GetAuthorsOfBookAsync(int idBook);
}