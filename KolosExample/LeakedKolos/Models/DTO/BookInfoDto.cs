namespace LeakedKolos.Models.DTO;

public record BookInfoDto(string Title, IEnumerable<AuthorDto> Authors);