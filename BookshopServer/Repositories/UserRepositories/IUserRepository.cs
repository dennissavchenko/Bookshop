using BookShopServer.Entities;

namespace BookShopServer.Repositories.UserRepositories;

/// <summary>
/// Defines the contract for data access operations related to user entities.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>The user entity if found, otherwise <c>null</c>.</returns>
    Task<User?> GetUserByIdAsync(int id);

    /// <summary>
    /// Retrieves all user entities.
    /// </summary>
    /// <returns>A collection of all users.</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Updates an existing user in the database.
    /// </summary>
    /// <param name="user">The user entity with updated information.</param>
    Task UpdateUserAsync(User user);

    /// <summary>
    /// Deletes a user from the database by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    Task DeleteUserAsync(int id);

    /// <summary>
    /// Checks if a user with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the user to check.</param>
    /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
    Task<bool> UserExistsAsync(int id);

    /// <summary>
    /// Checks if the provided email address is unique among existing users.
    /// </summary>
    /// <param name="email">The email address to check for uniqueness.</param>
    /// <returns><c>true</c> if the email is unique; otherwise, <c>false</c>.</returns>
    Task<bool> EmailUniqueAsync(string email);

    /// <summary>
    /// Checks if the provided username is unique among existing users.
    /// </summary>
    /// <param name="username">The username to check for uniqueness.</param>
    /// <returns><c>true</c> if the username is unique; otherwise, <c>false</c>.</returns>
    Task<bool> UsernameUniqueAsync(string username);

    /// <summary>
    /// Retrieves a collection of all users who are either customers or employees.
    /// </summary>
    /// <returns>A collection of customer and employee users.</returns>
    Task<IEnumerable<User>> GetAllCustomersAndEmployeesAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByUsernameAsync(string username);
}