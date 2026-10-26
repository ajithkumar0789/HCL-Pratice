
using System;
using Task1;
namespace Task1;

public class Program
{
    public static void Main(string[] args)
    {
        int order_id = 0;
        string customer_name = "";
        string product_name = "";
        int quantity = 0;
        decimal price = 0.0m;
        int count = 1;
        bool isValid = false;

        // Order ID verification
        while (!isValid)
        {
            if (count > 1)
            {
                Console.WriteLine(
                    "Invalid input. Please enter a valid, positive Order ID.");
            }
           
                Console.Write("Enter Order ID: ");
            

            isValid = int.TryParse(Console.ReadLine(), out order_id) && order_id > 0;
            count++;
        }

        // Customer name verification
        count = 1;
        isValid = false;

        while (!isValid)
        {
            if (count > 1)
            {
                Console.WriteLine(
                    "Customer name must not be empty.");
            }

            Console.Write("Enter Customer Name: ");
            customer_name = Console.ReadLine();


            isValid = !string.IsNullOrWhiteSpace(customer_name);
            count++;
        }

        //  product name verification
        count = 1;
        isValid = false;

        while (!isValid)
        {
            if (count > 1)
            {
                Console.WriteLine(
                    " product name must not be empty.");
            }

            Console.Write("Enter Product Name: ");
            product_name = Console.ReadLine();

            isValid = !string.IsNullOrWhiteSpace(product_name);

            count++;
        }

        // Quantity verification
        count = 1;
        isValid = false;

        while (!isValid)
        {
            if (count > 1)
            {
                Console.WriteLine(
                    "Invalid input. Please enter a valid, positive Quantity.");
            }
            
                Console.Write("Enter Quantity: ");
            

            isValid = int.TryParse(Console.ReadLine(), out quantity) && quantity > 0;
            count++;
        }

        // Unit price verification
        count = 1;
        isValid = false;

        while (!isValid)
        {
            if (count > 1)
            {
                Console.WriteLine(
                    "Invalid input. Please enter a valid, positive Price.");
            }
            
                Console.Write("Enter Unit Price: ");
            

            isValid = decimal.TryParse(Console.ReadLine(), out price)
                      && price > 0;
            count++;
        }

        //Total price and classification findings
        decimal total_price = quantity * price;
        string classification = "";
        if (total_price < 1000)
        {
            classification = "Small order";
        }
        else if (total_price <5000)
        {
            classification = "Medium Order";
        }
        else if (total_price < 10000)
        {
            classification = "Large Order";
        }
        else
        {
            classification = "Bulk Order";
        }
        Console.WriteLine("\n--- Order Details ---");
        Console.WriteLine("Order ID: " + order_id);
        Console.WriteLine("Customer Name: " + customer_name);
        Console.WriteLine("Product Name: " + product_name);
        Console.WriteLine("Quantity: " + quantity);
        Console.WriteLine("Unit Price: " + price);
        Console.WriteLine("Total Price: " + total_price);
        Console.WriteLine("Classification: " + classification);
    }
}