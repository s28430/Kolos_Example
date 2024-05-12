using System.ComponentModel.DataAnnotations;

namespace LeakedKolosgroupA.DTOs.ResponseDTOs;

public record BookInfoDto([Required] int Id, [Required] string Title, [Required] IEnumerable<string> Authors);