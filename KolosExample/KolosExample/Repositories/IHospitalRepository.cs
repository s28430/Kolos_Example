using KolosExample.Models;
using KolosExample.Models.DTO;

namespace KolosExample.Repositories;

public interface IHospitalRepository
{
    Task<IEnumerable<Prescription>> GetPrescriptionsAsync();
    Task<int> GetDoctorIdByLastNameAsync(string doctorLastName);
    Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorId(int idDoctor);
    Task<int> AddPrescriptionAsync(PostPrescriptionRequestDto prescriptionRequestDto);
    Task<Doctor?> GetDoctorByIdAsync(int idDoctor);
    Task<Patient?> GetPatientByIdAsync(int idPatient);
}