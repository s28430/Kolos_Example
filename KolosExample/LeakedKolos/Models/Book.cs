using System.ComponentModel.DataAnnotations;

namespace LeakedKolos.Models;

public class Book
{
    [Required]
    public int IdBook { get; set; }

    [Required] 
    [MaxLength(100)]
    public string Title { get; set; } = null!;
}