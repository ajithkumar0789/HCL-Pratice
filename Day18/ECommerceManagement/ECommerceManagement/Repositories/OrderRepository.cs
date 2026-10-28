
using ECommerceManagement.Helpers;
using ECommerceManagement.Models;
using Microsoft.Data.SqlClient;

namespace ECommerceManagement.Repositories;

public class OrderRepository
{
    public List<Order> GetAll()
    {
        List<Order> orders = new();

        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        string query = """
            SELECT OrderId, CustomerId, OrderDate, TotalAmount, OrderStatus
            FROM dbo.Orders
            ORDER BY OrderId DESC
            """;

        using SqlCommand command = new(query, connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            orders.Add(new Order
            {
                OrderId = reader.GetInt32(0),
                CustomerId = reader.GetInt32(1),
                OrderDate = reader.GetDateTime(2),
                TotalAmount = reader.GetDecimal(3),
                OrderStatus = reader.GetString(4)
            });
        }

        return orders;
    }

    public int CreateOrder(int customerId, List<OrderItem> items)
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException(
                "Add at least one product.");
        }

        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        using SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            decimal total = 0;

            // Check product prices and available stock.
            foreach (OrderItem item in items)
            {
                if (item.Quantity <= 0)
                {
                    throw new InvalidOperationException(
                        "Quantity must be greater than zero.");
                }

                string productQuery = """
                    SELECT Price, Stock
                    FROM dbo.Products WITH (UPDLOCK, HOLDLOCK)
                    WHERE ProductId = @ProductId AND IsActive = 1
                    """;

                decimal price;
                int stock;

                using (SqlCommand productCommand =
                    new(productQuery, connection, transaction))
                {
                    productCommand.Parameters.AddWithValue(
                        "@ProductId", item.ProductId);

                    using SqlDataReader reader =
                        productCommand.ExecuteReader();

                    if (!reader.Read())
                    {
                        throw new InvalidOperationException(
                            $"Product {item.ProductId} was not found or is inactive.");
                    }

                    price = reader.GetDecimal(0);
                    stock = reader.GetInt32(1);
                }

                if (stock < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {item.ProductId}.");
                }

                item.UnitPrice = price;
                total += price * item.Quantity;
            }

            // Check whether the customer exists and is active.
            string customerQuery = """
                SELECT COUNT(*)
                FROM dbo.Customers
                WHERE CustomerId = @Id AND IsActive = 1
                """;

            using (SqlCommand customerCommand =
                new(customerQuery, connection, transaction))
            {
                customerCommand.Parameters.AddWithValue(
                    "@Id", customerId);

                int customerCount =
                    (int)customerCommand.ExecuteScalar()!;

                if (customerCount == 0)
                {
                    throw new InvalidOperationException(
                        "Customer does not exist or is inactive.");
                }
            }

            // Create the order.
            string orderQuery = """
                INSERT INTO dbo.Orders
                    (CustomerId, OrderDate, TotalAmount, OrderStatus)
                OUTPUT INSERTED.OrderId
                VALUES
                    (@CustomerId, GETDATE(), @TotalAmount, 'Pending')
                """;

            int orderId;

            using (SqlCommand orderCommand =
                new(orderQuery, connection, transaction))
            {
                orderCommand.Parameters.AddWithValue(
                    "@CustomerId", customerId);

                orderCommand.Parameters.AddWithValue(
                    "@TotalAmount", total);

                orderId = (int)orderCommand.ExecuteScalar()!;
            }

            // Add order items and reduce stock.
            foreach (OrderItem item in items)
            {
                string itemQuery = """
                    INSERT INTO dbo.OrderItems
                        (OrderId, ProductId, Quantity, UnitPrice)
                    VALUES
                        (@OrderId, @ProductId, @Quantity, @UnitPrice);

                    UPDATE dbo.Products
                    SET Stock = Stock - @Quantity
                    WHERE ProductId = @ProductId
                      AND Stock >= @Quantity;

                    IF @@ROWCOUNT = 0
                        THROW 50001, 'Insufficient stock.', 1;
                    """;

                using SqlCommand itemCommand =
                    new(itemQuery, connection, transaction);

                itemCommand.Parameters.AddWithValue(
                    "@OrderId", orderId);

                itemCommand.Parameters.AddWithValue(
                    "@ProductId", item.ProductId);

                itemCommand.Parameters.AddWithValue(
                    "@Quantity", item.Quantity);

                itemCommand.Parameters.AddWithValue(
                    "@UnitPrice", item.UnitPrice);

                itemCommand.ExecuteNonQuery();
            }

            // Record the order creation in the audit log.
            string auditQuery = """
                INSERT INTO dbo.AuditLogs
                    (EntityName, EntityId, Action, Details, CreatedAt)
                VALUES
                    ('Order', @Id, 'Created', @Details, GETDATE())
                """;

            using (SqlCommand auditCommand =
                new(auditQuery, connection, transaction))
            {
                auditCommand.Parameters.AddWithValue("@Id", orderId);

                auditCommand.Parameters.AddWithValue(
                    "@Details", $"Order total: {total}");

                auditCommand.ExecuteNonQuery();
            }

            transaction.Commit();

            return orderId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public bool UpdateStatus(int orderId, string newStatus)
    {
        string[] validStatuses =
        {
            "Pending",
            "Confirmed",
            "Shipped",
            "Delivered",
            "Cancelled"
        };

        if (!validStatuses.Contains(newStatus))
        {
            throw new InvalidOperationException(
                "Invalid order status.");
        }

        using SqlConnection connection = DatabaseHelper.GetConnection();
        connection.Open();

        using SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            // Get the current status.
            string statusQuery = """
                SELECT OrderStatus
                FROM dbo.Orders WITH (UPDLOCK, HOLDLOCK)
                WHERE OrderId = @Id
                """;

            string? oldStatus;

            using (SqlCommand command =
                new(statusQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@Id", orderId);

                oldStatus = command.ExecuteScalar() as string;
            }

            if (oldStatus == null)
            {
                transaction.Rollback();
                return false;
            }

            if (oldStatus == "Cancelled" || oldStatus == "Delivered")
            {
                throw new InvalidOperationException(
                    "A cancelled or delivered order cannot be changed.");
            }

            if (newStatus == "Cancelled" && oldStatus == "Shipped")
            {
                throw new InvalidOperationException(
                    "A shipped order cannot be cancelled.");
            }

            // Restore stock if the order is cancelled.
            if (newStatus == "Cancelled")
            {
                string restoreStockQuery = """
                    UPDATE P
                    SET P.Stock = P.Stock + OI.Quantity
                    FROM dbo.Products AS P
                    INNER JOIN dbo.OrderItems AS OI
                        ON P.ProductId = OI.ProductId
                    WHERE OI.OrderId = @Id;
                    """;

                using SqlCommand restoreCommand =
                    new(restoreStockQuery, connection, transaction);

                restoreCommand.Parameters.AddWithValue("@Id", orderId);

                restoreCommand.ExecuteNonQuery();
            }

            // Update the order status.
            string updateQuery = """
                UPDATE dbo.Orders
                SET OrderStatus = @Status
                WHERE OrderId = @Id
                """;

            using (SqlCommand command =
                new(updateQuery, connection, transaction))
            {
                command.Parameters.AddWithValue("@Id", orderId);
                command.Parameters.AddWithValue("@Status", newStatus);

                command.ExecuteNonQuery();
            }

            // Record the status change.
            string auditQuery = """
                INSERT INTO dbo.AuditLogs
                    (EntityName, EntityId, Action, Details, CreatedAt)
                VALUES
                    ('Order', @Id, 'Status Updated', @Details, GETDATE())
                """;

            using (SqlCommand auditCommand =
                new(auditQuery, connection, transaction))
            {
                auditCommand.Parameters.AddWithValue("@Id", orderId);

                auditCommand.Parameters.AddWithValue(
                    "@Details", $"{oldStatus} -> {newStatus}");

                auditCommand.ExecuteNonQuery();
            }

            transaction.Commit();

            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
