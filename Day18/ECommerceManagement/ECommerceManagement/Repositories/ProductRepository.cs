
using ECommerceManagement.Helpers;
using ECommerceManagement.Models;
using Microsoft.Data.SqlClient;

namespace ECommerceManagement.Repositories;

public class ProductRepository
{
    public List<Product> GetAll()
    {
        List<Product> products = new();

        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            SELECT ProductId, ProductName, CategoryId, Price, Stock, IsActive
            FROM dbo.Products
            ORDER BY ProductId
            """;

        using SqlCommand command = new(query, connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                ProductId = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                CategoryId = reader.GetInt32(2),
                Price = reader.GetDecimal(3),
                Stock = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return products;
    }

    public void Add(Product product)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            INSERT INTO dbo.Products
                (ProductName, CategoryId, Price, Stock, IsActive)
            VALUES
                (@Name, @CategoryId, @Price, @Stock, @IsActive)
            """;

        using SqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@Name", product.ProductName);
        command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@Stock", product.Stock);
        command.Parameters.AddWithValue("@IsActive", product.IsActive);

        command.ExecuteNonQuery();
    }

    public bool Update(Product product)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            UPDATE dbo.Products
            SET ProductName = @Name,
                CategoryId = @CategoryId,
                Price = @Price,
                Stock = @Stock
            WHERE ProductId = @Id
            """;

        using SqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@Id", product.ProductId);
        command.Parameters.AddWithValue("@Name", product.ProductName);
        command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@Stock", product.Stock);

        return command.ExecuteNonQuery() > 0;
    }

    public bool Deactivate(int productId)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query =
            "UPDATE dbo.Products SET IsActive = 0 WHERE ProductId = @Id";

        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", productId);

        return command.ExecuteNonQuery() > 0;
    }

    public bool ChangeStock(int productId, int quantity)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            UPDATE dbo.Products
            SET Stock = Stock + @Quantity
            WHERE ProductId = @Id
              AND Stock + @Quantity >= 0
            """;

        using SqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@Id", productId);
        command.Parameters.AddWithValue("@Quantity", quantity);

        return command.ExecuteNonQuery() > 0;
    }
}
