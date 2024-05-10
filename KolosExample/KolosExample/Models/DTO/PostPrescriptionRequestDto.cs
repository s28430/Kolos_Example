using System.ComponentModel.DataAnnotations;

namespace KolosExample.Models.DTO;

public record PostPrescriptionRequestDto(
    [Required] DateTime Date, [Required] DateTime DueDate, [Required] int IdPatient, [Required] int IdDoctor);
