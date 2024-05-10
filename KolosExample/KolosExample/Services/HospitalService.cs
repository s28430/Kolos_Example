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

    private async Task ValidateDoctorExistsAsync(int idDoctor)
    {
        if (await _repository.GetDoctorByIdAsync(idDoctor) is null)
        {
            throw new 
                DoctorNotFoundException($"Doctor with id {idDoctor} does not exist.");
        }
    }
    
    private async Task ValidatePatientExistsAsync(int idPatient)
    {
        if (await _repository.GetPatientByIdAsync(idPatient) is null)
        {
            throw new 
                PatientNotFoundException($"Patient with id {idPatient} does not exist.");
        }
    }

    public async Task<Prescription> AddPrescriptionAsync(PostPrescriptionRequestDto prescriptionRequestDto)
    {
        if (prescriptionRequestDto.Date >= prescriptionRequestDto.DueDate)
        {
            throw new ArgumentException("Prescription's due date has to be older than its creation date.");
        }

        await ValidateDoctorExistsAsync(prescriptionRequestDto.IdDoctor);
        
        await ValidatePatientExistsAsync(prescriptionRequestDto.IdPatient);
        
        var prescriptionId = await _repository.AddPrescriptionAsync(prescriptionRequestDto);

        if (prescriptionId < 0)
        {
            throw new Exception("Internal server error occurred.");
        }
        
        return new Prescription
        {
            IdPrescription = prescriptionId,
            Date = prescriptionRequestDto.Date,
            DueDate = prescriptionRequestDto.DueDate,
            IdPatient = prescriptionRequestDto.IdPatient,
            IdDoctor = prescriptionRequestDto.IdDoctor
        };
    }
}