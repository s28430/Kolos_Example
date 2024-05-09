using KolosExample.Models;

namespace KolosExample.Services;

public interface IHospitalService
{
    Task<IEnumerable<Prescription>> GetPrescriptionsAsync();
}