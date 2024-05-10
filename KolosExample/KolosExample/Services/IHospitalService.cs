using KolosExample.Models;
using KolosExample.Models.DTO;

namespace KolosExample.Services;

public interface IHospitalService
{
    Task<IEnumerable<Prescription>> GetPrescriptionsAsync(string? doctorLastName);
    Task<Prescription> AddPrescriptionAsync(PostPrescriptionRequestDto prescriptionRequestDto);
}