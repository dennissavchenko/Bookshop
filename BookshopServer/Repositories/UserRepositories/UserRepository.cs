using BookShopServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShopServer.Repositories.UserRepositories;

/// <summary>
/// Provides data access methods for managing user entities in the bookshop system.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly Context _context;
    
    /// <summary>
    /// Initializes a new instance of the <c>UserRepository</c> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier, including their associated customer and employee data.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user entity if found.</returns>
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(x => x.Employee)
            .Include(x => x.Customer)
            .FirstAsync(x => x.Id == id);
    }

    /// <summary>
    /// Retrieves all user entities, including their associated customer and employee data.
    /// </summary>
    /// <returns>A collection of all users.</returns>
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(x => x.Customer)
            .Include(x => x.Employee)
            .ToListAsync();
    }
    
    /// <summary>
    /// Updates an existing user in the database.
    /// </summary>
    /// <param name="user">The user entity with updated information.</param>
    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Deletes a user from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FirstAsync(x => x.Id == id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    /// <summary>
    /// Checks if a user with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the user to check.</param>
    /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
    public async Task<bool> UserExistsAsync(int id)
    {
        return await _context.Users.AnyAsync(x => x.Id == id);
    }
    
    /// <summary>
    /// Checks if the provided email address is unique among existing users (case-insensitive).
    /// </summary>
    /// <param name="email">The email address to check for uniqueness.</param>
    /// <returns><c>true</c> if the email is unique; otherwise, <c>false</c>.</returns>
    public async Task<bool> EmailUniqueAsync(string email)
    {
        return !await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }
    
    /// <summary>
    /// Checks if the provided username is unique among existing users (case-insensitive).
    /// </summary>
    /// <param name="username">The username to check for uniqueness.</param>
    /// <returns><c>true</c> if the username is unique; otherwise, <c>false</c>.</returns>
    public async Task<bool> UsernameUniqueAsync(string username)
    {
        return !await _context.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower());
    }
    
    /// <summary>
    /// Retrieves a collection of all users who are associated with either a customer or an employee profile.
    /// </summary>
    /// <returns>A collection of users who are customers or employees.</returns>
    public async Task<IEnumerable<User>> GetAllCustomersAndEmployeesAsync()
    {
        return await _context.Users.Where(u => u.Customer != null || u.Employee != null)
            .Include(x => x.Customer)
            .Include(x => x.Employee)
            .ToListAsync();
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.Customer)
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(x => x.Customer)
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
    }
    
}