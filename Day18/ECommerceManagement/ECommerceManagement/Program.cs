using ECommerceManagement.Models;
using ECommerceManagement.Services;

namespace ECommerceManagement;

public class Program
{
    private static readonly CategoryService categoryService = new();
    private static readonly ProductService productService = new();
    private static readonly CustomerService customerService = new();
    private static readonly OrderService orderService = new();

    public static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("======================================");
            Console.WriteLine("   E-COMMERCE MANAGEMENT SYSTEM");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Manage Categories");
            Console.WriteLine("2. Manage Products");
            Console.WriteLine("3. Manage Customers");
            Console.WriteLine("4. Manage Orders");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";

            try
            {
                switch (choice)
                {
                    case "1":
                        CategoryMenu();
                        break;
                    case "2":
                        ProductMenu();
                        break;
                    case "3":
                        CustomerMenu();
                        break;
                    case "4":
                        OrderMenu();
                        break;
                    case "0":
                        Console.WriteLine("Thank you for using the system.");
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Pause();
            }
        }
    }

    private static void CategoryMenu()
    {
        Console.Clear();
        Console.WriteLine("=== CATEGORY MANAGEMENT ===");
        Console.WriteLine("1. View Categories");
        Console.WriteLine("2. Add Category");
        Console.WriteLine("3. Update Category");
        Console.WriteLine("4. Deactivate Category");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1":
                foreach (Category c in categoryService.GetAll())
                    Console.WriteLine(
                        $"{c.CategoryId} | {c.CategoryName} | {c.Description} | Active: {c.IsActive}");
                break;

            case "2":
                Category category = new();
                Console.Write("Category name: ");
                category.CategoryName = ReadText();
                Console.Write("Description: ");
                category.Description = ReadText();

                categoryService.Add(category);
                Console.WriteLine("Category added.");
                break;

            case "3":
                Category update = new();
                update.CategoryId = ReadInt("Category ID: ");
                Console.Write("New category name: ");
                update.CategoryName = ReadText();
                Console.Write("New description: ");
                update.Description = ReadText();

                Console.WriteLine(categoryService.Update(update)
                    ? "Category updated."
                    : "Category not found.");
                break;

            case "4":
                Console.WriteLine(categoryService.Deactivate(
                    ReadInt("Category ID: "))
                    ? "Category deactivated."
                    : "Category not found.");
                break;
        }

        Pause();
    }

    private static void ProductMenu()
    {
        Console.Clear();
        Console.WriteLine("=== PRODUCT MANAGEMENT ===");
        Console.WriteLine("1. View Products");
        Console.WriteLine("2. Add Product");
        Console.WriteLine("3. Update Product");
        Console.WriteLine("4. Deactivate Product");
        Console.WriteLine("5. Adjust Stock");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1":
                foreach (Product p in productService.GetAll())
                    Console.WriteLine(
                        $"{p.ProductId} | {p.ProductName} | Category: {p.CategoryId} | Price: {p.Price:C} | Stock: {p.Stock} | Active: {p.IsActive}");
                break;

            case "2":
                Product product = ReadProductDetails();
                productService.Add(product);
                Console.WriteLine("Product added.");
                break;

            case "3":
                Product update = ReadProductDetails();
                update.ProductId = ReadInt("Product ID to update: ");

                Console.WriteLine(productService.Update(update)
                    ? "Product updated."
                    : "Product not found.");
                break;

            case "4":
                Console.WriteLine(productService.Deactivate(
                    ReadInt("Product ID: "))
                    ? "Product deactivated."
                    : "Product not found.");
                break;

            case "5":
                int id = ReadInt("Product ID: ");
                int adjustment = ReadInt(
                    "Stock adjustment (+ to add, - to remove): ");

                Console.WriteLine(productService.ChangeStock(id, adjustment)
                    ? "Stock updated."
                    : "Product not found or insufficient stock.");
                break;
        }

        Pause();
    }

    private static Product ReadProductDetails()
    {
        Product product = new();

        Console.Write("Product name: ");
        product.ProductName = ReadText();

        product.CategoryId = ReadInt("Category ID: ");
        product.Price = ReadDecimal("Price: ");
        product.Stock = ReadInt("Initial/current stock: ");

        return product;
    }

    private static void CustomerMenu()
    {
        Console.Clear();
        Console.WriteLine("=== CUSTOMER MANAGEMENT ===");
        Console.WriteLine("1. View Customers");
        Console.WriteLine("2. Add Customer");
        Console.WriteLine("3. Update Customer");
        Console.WriteLine("4. Deactivate Customer");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1":
                foreach (Customer c in customerService.GetAll())
                    Console.WriteLine(
                        $"{c.CustomerId} | {c.CustomerName} | {c.Email} | {c.PhoneNumber} | Active: {c.IsActive}");
                break;

            case "2":
                Customer customer = ReadCustomerDetails();
                customerService.Add(customer);
                Console.WriteLine("Customer added.");
                break;

            case "3":
                Customer update = ReadCustomerDetails();
                update.CustomerId = ReadInt("Customer ID to update: ");

                Console.WriteLine(customerService.Update(update)
                    ? "Customer updated."
                    : "Customer not found.");
                break;

            case "4":
                Console.WriteLine(customerService.Deactivate(
                    ReadInt("Customer ID: "))
                    ? "Customer deactivated."
                    : "Customer not found.");
                break;
        }

        Pause();
    }

    private static Customer ReadCustomerDetails()
    {
        Customer customer = new();

        Console.Write("Customer name: ");
        customer.CustomerName = ReadText();

        Console.Write("Email: ");
        customer.Email = ReadText();

        Console.Write("Phone number (10 digits): ");
        customer.PhoneNumber = ReadText();

        Console.Write("Address: ");
        customer.Address = ReadText();

        return customer;
    }

    private static void OrderMenu()
    {
        Console.Clear();
        Console.WriteLine("=== ORDER MANAGEMENT ===");
        Console.WriteLine("1. View Orders");
        Console.WriteLine("2. Create Order (one product)");
        Console.WriteLine("3. Update Order Status");
        Console.WriteLine("0. Back");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1":
                foreach (Order o in orderService.GetAll())
                    Console.WriteLine(
                        $"Order: {o.OrderId} | Customer: {o.CustomerId} | Date: {o.OrderDate} | Total: {o.TotalAmount:C} | Status: {o.OrderStatus}");
                break;

            case "2":
                int customerId = ReadInt("Customer ID: ");
                int productId = ReadInt("Product ID: ");
                int quantity = ReadInt("Quantity: ");

                List<OrderItem> items = new()
                {
                    new OrderItem
                    {
                        ProductId = productId,
                        Quantity = quantity
                    }
                };

                int orderId = orderService.CreateOrder(customerId, items);
                Console.WriteLine($"Order created successfully. Order ID: {orderId}");
                break;

            case "3":
                int id = ReadInt("Order ID: ");

                Console.WriteLine("Statuses: Pending, Confirmed, Shipped, Delivered, Cancelled");
                Console.Write("New status: ");
                string status = ReadText();

                Console.WriteLine(orderService.UpdateStatus(id, status)
                    ? "Order status updated."
                    : "Order not found.");
                break;
        }

        Pause();
    }

    private static string ReadText()
    {
        return Console.ReadLine()?.Trim() ?? "";
    }

    private static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value))
                return value;

            Console.WriteLine("Enter a valid whole number.");
        }
    }

    private static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (decimal.TryParse(Console.ReadLine(), out decimal value))
                return value;

            Console.WriteLine("Enter a valid amount.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}