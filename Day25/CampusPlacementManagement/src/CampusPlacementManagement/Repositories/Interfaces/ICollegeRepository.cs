
using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces;

public interface ICollegeRepository
{
    Task<IEnumerable<College>> GetAllAsync();

    Task<College?> GetByIdAsync(int id);

    Task AddAsync(College college);

    void Update(College college);

    void Delete(College college);

    Task SaveChangesAsync();
}
