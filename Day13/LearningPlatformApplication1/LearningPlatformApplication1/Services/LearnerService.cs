
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class LearnerService
{
    private readonly AppDbContext db;

    public LearnerService(AppDbContext context)
    {
        db = context;
    }

    public async Task<List<Learner>> GetAllLearners()
    {
        return await db.Learners.ToListAsync();
    }

    public async Task<Learner?> GetLearnerById(string id)
    {
        return await db.Learners.FindAsync(id);
    }

    public async Task<string?> RegisterLearner(Learner learner)
    {
        if (string.IsNullOrWhiteSpace(learner.LearnerId) ||
            string.IsNullOrWhiteSpace(learner.Name))
        {
            return "Learner ID and name are required.";
        }

        if (!ValidContact(learner.ContactNumber))
            return "Contact number must contain exactly 10 digits.";

        if (string.IsNullOrWhiteSpace(learner.Email))
            return "Email is required.";

        if (await db.Learners.FindAsync(learner.LearnerId) != null)
            return "Learner ID already exists.";

        db.Learners.Add(learner);
        await db.SaveChangesAsync();

        return null;
    }

    public async Task<string?> UpdateLearner(
        string id, Learner updated)
    {
        Learner? learner = await db.Learners.FindAsync(id);

        if (learner == null)
            return "Learner not found.";

        if (string.IsNullOrWhiteSpace(updated.Name) ||
            string.IsNullOrWhiteSpace(updated.Email))
        {
            return "Name and email are required.";
        }

        if (!ValidContact(updated.ContactNumber))
            return "Contact number must contain exactly 10 digits.";

        learner.Name = updated.Name;
        learner.ContactNumber = updated.ContactNumber;
        learner.Email = updated.Email;

        await db.SaveChangesAsync();

        return null;
    }

    public async Task<string?> DeleteLearner(string id)
    {
        Learner? learner = await db.Learners.FindAsync(id);

        if (learner == null)
            return "Learner not found.";

        bool hasHistory = await db.Enrollments
            .AnyAsync(e => e.LearnerId == id);

        if (hasHistory)
            return "Cannot delete a learner with enrollment history.";

        db.Learners.Remove(learner);
        await db.SaveChangesAsync();

        return null;
    }

    private static bool ValidContact(string contact)
    {
        if (string.IsNullOrWhiteSpace(contact) ||
            contact.Length != 10)
        {
            return false;
        }

        foreach (char digit in contact)
        {
            if (!char.IsDigit(digit))
                return false;
        }

        return true;
    }
}
