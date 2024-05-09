using KolosExample.Models;

namespace KolosExample.Repositories;

public interface IHospitalRepository
{
    Task<IEnumerable<Prescription>> GetPrescriptionsAsync();
}