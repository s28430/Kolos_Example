using LeakedKolos.Repositories;

namespace LeakedKolos.Services;

public class BooksService(IBooksRepository repository) : IBooksService
{
    private IBooksRepository _repository = repository;
}