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

    public async Task<IEnumerable<Author>?> GetBooksAuthorsByBookIdAsync(int idBook)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT a.PK, a.first_name, a.last_name " +
                          "FROM authors a " +
                          "JOIN books_authors ba ON a.PK = ba.FK_author " +
                          "WHERE ba.FK_book = @idBook";
        cmd.Parameters.AddWithValue("idBook", idBook);
        
        await using var dr = await cmd.ExecuteReaderAsync();

        var authors = new List<Author>();
        while (await dr.ReadAsync())
        {
            var author = new Author
            {
                IdAuthor = (int)dr["PK"],
                FirstName = dr["first_name"].ToString() ?? string.Empty,
                LastName = dr["last_name"].ToString() ?? string.Empty
            };
            authors.Add(author);
        }

        return authors;
    }
}