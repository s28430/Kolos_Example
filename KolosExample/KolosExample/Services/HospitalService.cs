using KolosExample.Exceptions;
using KolosExample.Models;
using KolosExample.Repositories;

namespace KolosExample.Services;

public class HospitalService(IHospitalRepository repository) : IHospitalService
{
    private readonly IHospitalRepository _repository = repository;
    
    public async Task<IEnumerable<Prescription>> GetPrescriptionsAsync(string? doctorLastName)
    {
        if (doctorLastName is null)
        {
            return await _repository.GetPrescriptionsAsync();
        }

        var idDoctor = await _repository.GetDoctorIdByLastNameAsync(doctorLastName);
        if (idDoctor < 0)
        {
            throw new DoctorNotFoundException($"Doctor with last name '{doctorLastName}' was not found");
        }

        return await _repository.GetPrescriptionsByDoctorId(idDoctor);
    }
}