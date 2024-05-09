using System.Data.SqlClient;
using System.Runtime.InteropServices.JavaScript;
using KolosExample.Exceptions;
using KolosExample.Models;

namespace KolosExample.Repositories;

public class HospitalRepository(IConfiguration configuration) : IHospitalRepository
{
    private readonly IConfiguration _configuration = configuration;
    
    public async Task<IEnumerable<Prescription>> GetPrescriptionsAsync()
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT idPrescription, [Date], DueDate, IdPatient, idDoctor " +
                          "FROM Prescription";
        
        await using var dr = await cmd.ExecuteReaderAsync();
        var result = new List<Prescription>();

        while (dr.Read())
        {
            var prescription = new Prescription()
            {
                IdPrescription = (int)dr["idPrescription"],
                Date = DateTime.Parse(dr["date"].ToString() ?? throw new
                    InvalidDatabaseEntryException("Prescription table contains an invalid entry.")),
                DueDate = DateTime.Parse(dr["dueDate"].ToString() ?? throw new
                    InvalidDatabaseEntryException("Prescription table contains an invalid entry.")),
                IdDoctor = (int)dr["IdDoctor"],
                IdPatient = (int)dr["IdPatient"]
            };
            result.Add(prescription);
        }

        return result;
    }
}