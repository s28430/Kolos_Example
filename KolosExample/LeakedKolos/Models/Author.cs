using System.ComponentModel.DataAnnotations;

namespace LeakedKolos.Models;

public class Author
{
    [Required]
    public int IdAuthor { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)] 
    public string LastName { get; set; } = null!;
}