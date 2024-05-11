using System.Data.SqlClient;
using LeakedKolos.Models;

namespace LeakedKolos.Repositories;

public class BooksRepository(IConfiguration configuration) : IBooksRepository
{
    private readonly IConfiguration _configuration = configuration;
    
    public async Task<Book?> GetBookByIdAsync(int idBook)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * " +
                          "FROM books " +
                          "WHERE PK = @idBook";
        cmd.Parameters.AddWithValue("idBook", idBook);

        await using var dr = await cmd.ExecuteReaderAsync();

        if (!await dr.ReadAsync())
        {
            return null;
        }

        var book = new Book
        {
            IdBook = (int)dr["PK"],
            Title = dr["title"].ToString() ?? string.Empty
        };
        return book;
    }
}