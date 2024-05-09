using KolosExample.Models;
using KolosExample.Repositories;

namespace KolosExample.Services;

public class HospitalService(IHospitalRepository repository) : IHospitalService
{
    private readonly IHospitalRepository _repository = repository;
    
    public async Task<IEnumerable<Prescription>> GetPrescriptionsAsync()
    {
        return await _repository.GetPrescriptionsAsync();
    }
}