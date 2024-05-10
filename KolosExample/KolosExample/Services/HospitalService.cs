using KolosExample.Exceptions;
using KolosExample.Models;
using KolosExample.Models.DTO;
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

    public async Task<int> AddPrescriptionAsync(PostPrescriptionRequestDto prescriptionRequestDto)
    {
        if (prescriptionRequestDto.Date >= prescriptionRequestDto.DueDate)
        {
            throw new ArgumentException("Prescription's due date has to be older than its creation date.");
        }

        if (await _repository.GetDoctorByIdAsync(prescriptionRequestDto.IdDoctor) is null)
        {
            throw new 
                DoctorNotFoundException($"Doctor with id {prescriptionRequestDto.IdDoctor} does not exist.");
        }
        
        if (await _repository.GetPatientByIdAsync(prescriptionRequestDto.IdPatient) is null)
        {
            throw new 
                PatientNotFoundException($"Patient with id {prescriptionRequestDto.IdPatient} does not exist.");
        }
        
        return await _repository.AddPrescriptionAsync(prescriptionRequestDto);
    }
}