using LeakedKolos.Models.DTO;

namespace LeakedKolos.Services;

public interface IBooksService
{
    public Task<ResponseBookDto> GetBooksInfoByIdAsync(int idBook);
}