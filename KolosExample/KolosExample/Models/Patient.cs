using System.ComponentModel.DataAnnotations;

namespace KolosExample.Models;

public class Patient
{
    [Required]
    public int IdPatient { get; set; }

    [Required] 
    public string FirstName { get; set; } = null!;

    [Required] 
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }
}