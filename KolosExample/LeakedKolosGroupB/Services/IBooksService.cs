using LeakedKolos.Models.DTO;

namespace LeakedKolos.Services;

public interface IBooksService
{ 
    public Task<object> GetBooksInfoByIdAsync(int idBook);

    public Task<object> AddBookWithAuthors(BookInfoDto bookInfoDto);
}