using LeakedKolosgroupA.Models;
using Microsoft.Data.SqlClient;

namespace LeakedKolosgroupA.Repositories;

public class BookRepository(IConfiguration configuration) : IBookRepository
{
    private readonly IConfiguration _configuration = configuration;
    
    public async Task<IEnumerable<Author>> GetAuthorsOfBookAsync(int idBook)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT a.PK, a.first_name, a.last_name " +
                          "FROM authors a " +
                          "JOIN book_authors ba ON a.PK = ba.FK_author " +
                          "WHERE ba.FK_book = @idBook";
        cmd.Parameters.AddWithValue("idBook", idBook);

        await using var dr = await cmd.ExecuteReaderAsync();
        List<Author> result = [];
        
        while (await dr.ReadAsync())
        {
            var author = new Author
            {
                IdAuthor = (int)dr["PK"],
                FirstName = dr["first_name"].ToString()!,
                LastName = dr["last_name"].ToString()!
            };
            result.Add(author);
        }

        return result;
    }
}