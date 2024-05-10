using KolosExample.Models;

namespace KolosExample.Repositories;

public interface IHospitalRepository
{
    Task<IEnumerable<Prescription>> GetPrescriptionsAsync();
    Task<int> GetDoctorIdByLastNameAsync(string doctorLastName);
    Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorId(int idDoctor);
}