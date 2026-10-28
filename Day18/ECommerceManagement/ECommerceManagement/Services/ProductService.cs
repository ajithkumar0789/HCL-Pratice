using ECommerceManagement.Models;
using ECommerceManagement.Repositories;

namespace ECommerceManagement.Services;

public class ProductService
{
    private readonly ProductRepository repository = new();

    public List<Product> GetAll() => repository.GetAll();

    public void Add(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.ProductName))
            throw new ArgumentException("Product name is required.");

        if (product.CategoryId <= 0 || product.Price <= 0 || product.Stock < 0)
            throw new ArgumentException(
                "Enter a valid category, positive price and non-negative stock.");

        repository.Add(product);
    }

    public bool Update(Product product)
    {
        if (product.ProductId <= 0 ||
            string.IsNullOrWhiteSpace(product.ProductName) ||
            product.CategoryId <= 0 ||
            product.Price <= 0 ||
            product.Stock < 0)
            throw new ArgumentException("Enter valid product details.");

        return repository.Update(product);
    }

    public bool Deactivate(int id) => repository.Deactivate(id);

    public bool ChangeStock(int id, int quantity)
    {
        if (id <= 0 || quantity == 0)
            throw new ArgumentException(
                "Enter a valid product ID and non-zero stock adjustment.");

        return repository.ChangeStock(id, quantity);
    }
}
