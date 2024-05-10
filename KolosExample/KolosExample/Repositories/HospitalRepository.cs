using System.Data.Common;
using System.Data.SqlClient;
using KolosExample.Exceptions;
using KolosExample.Models;
using KolosExample.Models.DTO;

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
            var prescription = new Prescription
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

    public async Task<int> GetDoctorIdByLastNameAsync(string doctorLastName)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT idDoctor " +
                          "FROM doctor " +
                          "WHERE lastName = @lastName";
        cmd.Parameters.AddWithValue("lastName", doctorLastName);

        await using var dr = await cmd.ExecuteReaderAsync();

        if (await dr.ReadAsync())
        {
            return (int)dr["idDoctor"];
        }
        return -1;
    }

    public async Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorId(int idDoctor)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT p.idPrescription, p.Date, p.DueDate, p.IdPatient, p.idDoctor " +
                          "FROM prescription p " +
                          "JOIN doctor d ON d.idDoctor = p.idDoctor " +
                          "WHERE d.idDoctor = @idDoctor";
        cmd.Parameters.AddWithValue("idDoctor", idDoctor);

        List<Prescription> result = [];
        
        await using var dr = await cmd.ExecuteReaderAsync();
        while (await dr.ReadAsync())
        {
            var prescription = new Prescription
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

    public async Task<int> AddPrescriptionAsync(PostPrescriptionRequestDto prescriptionRequestDto)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        
        await using var transaction = (SqlTransaction) await conn.BeginTransactionAsync();
        cmd.Transaction = transaction;

        cmd.CommandText = "INSERT INTO prescription([date], dueDate, idPatient, idDoctor) " +
                          "VALUES (@date, @dueDate, @idPatient, @idDoctor);" +
                          "SELECT SCOPE_IDENTITY()";

        cmd.Parameters.AddWithValue("date", prescriptionRequestDto.Date);
        cmd.Parameters.AddWithValue("dueDate", prescriptionRequestDto.DueDate);
        cmd.Parameters.AddWithValue("idPatient", prescriptionRequestDto.IdPatient);
        cmd.Parameters.AddWithValue("idDoctor", prescriptionRequestDto.IdDoctor);
        
        try
        {

            var result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            await transaction.CommitAsync();
            return result;
        }
        catch (DbException)
        {
            await transaction.RollbackAsync();
            return -1;
        }
        
    }
}