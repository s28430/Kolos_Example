using System.ComponentModel.DataAnnotations;

namespace KolosExample.Models;

public class Prescription
{
    [Required]
    public int IdPrescription { get; set; }

    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required] 
    public int IdPatient { get; set; }
    
    [Required]
    public int IdDoctor { get; set; }
}