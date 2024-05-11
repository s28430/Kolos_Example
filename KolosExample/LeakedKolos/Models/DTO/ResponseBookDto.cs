namespace LeakedKolos.Models.DTO;

public record ResponseBookDto(int Id, string Title, IEnumerable<AuthorDto> Authors);