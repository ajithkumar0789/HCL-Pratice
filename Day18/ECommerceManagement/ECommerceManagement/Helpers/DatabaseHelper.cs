
using Microsoft.Data.SqlClient;

namespace ECommerceManagement.Helpers;

public static class DatabaseHelper
{
    private static readonly string ConnectionString =
    @"Server=AJITH\SQLEXPRESS;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}
