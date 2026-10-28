using ECommerceManagement.Models;
using ECommerceManagement.Repositories;

namespace ECommerceManagement.Services;

public class CategoryService
{
    private readonly CategoryRepository repository = new();

    public List<Category> GetAll() => repository.GetAll();

    public void Add(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.CategoryName))
            throw new ArgumentException("Category name is required.");

        repository.Add(category);
    }

    public bool Update(Category category)
    {
        if (category.CategoryId <= 0 ||
            string.IsNullOrWhiteSpace(category.CategoryName))
            throw new ArgumentException("Enter a valid category ID and name.");

        return repository.Update(category);
    }

    public bool Deactivate(int id) => repository.Deactivate(id);
}