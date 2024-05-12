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

    public async Task<int> AddBookAsync(string title, IEnumerable<int> authorsIdArray)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        await using var transaction = (SqlTransaction) await conn.BeginTransactionAsync();
        cmd.Transaction = transaction;

        int insertedBookId;
        cmd.CommandText = "INSERT INTO books(title) " +
                          "VALUES (@title);" +
                          "SELECT SCOPE_IDENTITY()";
        cmd.Parameters.AddWithValue("title", title);
        try
        {
            insertedBookId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return -1;
        }

        cmd.CommandText = "INSERT INTO books_authors (FK_book, FK_author) " +
                          "VALUES   (@idBook, @idAuthor)";

        try
        {
            foreach (var authorId in authorsIdArray)
            {
                cmd.Parameters.AddWithValue("idBook", insertedBookId);
                cmd.Parameters.AddWithValue("idAuthor", authorId);

                await cmd.ExecuteScalarAsync();
                cmd.Parameters.Clear();
            }
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return -1;
        }

        await transaction.CommitAsync();
        return insertedBookId;
    }

    public async Task<Author?> GetAuthorByNameAsync(string firstName, string lastName)
    {
        await using var conn = new SqlConnection(_configuration["conn-string"]);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * " +
                          "FROM authors " +
                          "WHERE first_name = @firstName AND last_name = @lastName";
        cmd.Parameters.AddWithValue("firstName", firstName);
        cmd.Parameters.AddWithValue("lastName", lastName);

        await using var dr = await cmd.ExecuteReaderAsync();
        if (!await dr.ReadAsync())
        {
            return null;
        }

        var author = new Author
        {
            IdAuthor = (int)dr["PK"],
            FirstName = dr["first_name"].ToString() ?? string.Empty,
            LastName = dr["last_name"].ToString() ?? string.Empty
        };
        return author;
    }
}