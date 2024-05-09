using System.ComponentModel.DataAnnotations;

namespace KolosExample.Models;

public class Doctor
{
    [Required] [Key] public int IdDoctor { get; set; }

    [Required] [MaxLength(100)] public string FirstName { get; set; } = "";

    [Required] [MaxLength(100)] public string LastName { get; set; } = "";
    
    [Required] [MaxLength(100)] public string Email { get; set; } = "";
}