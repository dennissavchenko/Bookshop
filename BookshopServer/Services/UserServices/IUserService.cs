using BookShopServer.DTOs.UserDTOs;
using BookShopServer.Entities;

namespace BookShopServer.Services.UserServices;

/// <summary>
/// Defines the contract for services that manage user-related operations,
/// including user deletion, retrieval, and role management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Deletes a user from the system based on their unique identifier.
    /// This operation typically involves anonymizing or deactivating the user's data.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    Task DeleteUserAsync(int id);

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>A <see cref="UserDto"/> object representing the user.</returns>
    Task<UserDto> GetUserByIdAsync(int id);

    /// <summary>
    /// Retrieves a collection of all users in the system.
    /// </summary>
    /// <returns>A collection of <see cref="UserDto"/> objects representing all users.</returns>
    Task<IEnumerable<UserDto>> GetAllUsersAsync();

    /// <summary>
    /// Adds a customer role to an existing user.
    /// This allows a user to perform customer-specific actions.
    /// </summary>
    /// <param name="customerRoleDto">The DTO containing information needed to assign the customer role.</param>
    Task AddCustomerRoleAsync(CustomerRoleDto customerRoleDto);

    /// <summary>
    /// Adds an employee role to an existing user.
    /// This allows a user to perform employee-specific actions.
    /// </summary>
    /// <param name="employeeRoleDto">The DTO containing information needed to assign the employee role.</param>
    Task AddEmployeeRoleAsync(EmployeeRoleDto employeeRoleDto);

    /// <summary>
    /// Deletes the customer role from a user.
    /// The user will no longer be able to perform customer-specific actions.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom to remove the customer role.</param>
    Task DeleteCustomerRoleAsync(int userId);

    /// <summary>
    /// Deletes the employee role from a user.
    /// The user will no longer be able to perform employee-specific actions.
    /// </summary>
    /// <param name="userId">The unique identifier of the user from whom to remove the employee role.</param>
    Task DeleteEmployeeRoleAsync(int userId);

    Task<User?> ValidateUserAsync(string usernameOrEmail, string password);
}