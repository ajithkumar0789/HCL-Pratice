
using ECommerceManagement.Helpers;
using ECommerceManagement.Models;
using Microsoft.Data.SqlClient;

namespace ECommerceManagement.Repositories;

public class CustomerRepository
{
    public List<Customer> GetAll()
    {
        List<Customer> customers = new();

        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            SELECT CustomerId, CustomerName, Email, PhoneNumber,
                   Address, CreatedAt, IsActive
            FROM dbo.Customers
            ORDER BY CustomerId
            """;

        using SqlCommand command = new(query, connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            customers.Add(new Customer
            {
                CustomerId = reader.GetInt32(0),
                CustomerName = reader.GetString(1),
                Email = reader.GetString(2),
                PhoneNumber = reader.GetString(3),
                Address = reader.IsDBNull(4) ? null : reader.GetString(4),
                CreatedAt = reader.GetDateTime(5),
                IsActive = reader.GetBoolean(6)
            });
        }

        return customers;
    }

    public void Add(Customer customer)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            INSERT INTO dbo.Customers
                (CustomerName, Email, PhoneNumber, Address, CreatedAt, IsActive)
            VALUES
                (@Name, @Email, @Phone, @Address, GETDATE(), 1)
            """;

        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Name", customer.CustomerName);
        command.Parameters.AddWithValue("@Email", customer.Email);
        command.Parameters.AddWithValue("@Phone", customer.PhoneNumber);
        command.Parameters.AddWithValue("@Address",
            (object?)customer.Address ?? DBNull.Value);

        command.ExecuteNonQuery();
    }

    public bool Update(Customer customer)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            UPDATE dbo.Customers
            SET CustomerName = @Name,
                Email = @Email,
                PhoneNumber = @Phone,
                Address = @Address
            WHERE CustomerId = @Id
            """;

        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", customer.CustomerId);
        command.Parameters.AddWithValue("@Name", customer.CustomerName);
        command.Parameters.AddWithValue("@Email", customer.Email);
        command.Parameters.AddWithValue("@Phone", customer.PhoneNumber);
        command.Parameters.AddWithValue("@Address",
            (object?)customer.Address ?? DBNull.Value);

        return command.ExecuteNonQuery() > 0;
    }

    public bool Deactivate(int customerId)
    {
        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query =
            "UPDATE dbo.Customers SET IsActive = 0 WHERE CustomerId = @Id";

        using SqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@Id", customerId);

        return command.ExecuteNonQuery() > 0;
    }
}
