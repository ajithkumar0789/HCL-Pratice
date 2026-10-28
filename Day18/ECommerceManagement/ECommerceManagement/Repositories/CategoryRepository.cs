
using ECommerceManagement.Helpers;
using ECommerceManagement.Models;
using Microsoft.Data.SqlClient;

namespace ECommerceManagement.Repositories;

public class CategoryRepository
{
    public List<Category> GetAll()
    {
        List<Category> categories = new();

        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            SELECT CategoryId, CategoryName, Description, IsActive
            FROM dbo.Categories
            ORDER BY CategoryId
            """;

        using SqlCommand command = new(query, connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            categories.Add(new Category
            {
                CategoryId = reader.GetInt32(0),
                CategoryName = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }

        return categories;
    }

    public void Add(Category category)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            INSERT INTO dbo.Categories
                (CategoryName, Description, IsActive)
            VALUES
                (@Name, @Description, @IsActive)
            """;

        using SqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@Name", category.CategoryName);
        command.Parameters.AddWithValue("@Description",
            (object?)category.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@IsActive", category.IsActive);

        command.ExecuteNonQuery();
    }

    public bool Update(Category category)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            UPDATE dbo.Categories
            SET CategoryName = @Name,
                Description = @Description
            WHERE CategoryId = @Id
            """;

        using SqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@Id", category.CategoryId);
        command.Parameters.AddWithValue("@Name", category.CategoryName);
        command.Parameters.AddWithValue("@Description",
            (object?)category.Description ?? DBNull.Value);

        return command.ExecuteNonQuery() > 0;
    }

    public bool Deactivate(int categoryId)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query =
            "UPDATE dbo.Categories SET IsActive = 0 WHERE CategoryId = @Id";

        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", categoryId);

        return command.ExecuteNonQuery() > 0;
    }
}
