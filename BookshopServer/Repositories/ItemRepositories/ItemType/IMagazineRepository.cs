using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories.ItemType;

/// <summary>
/// Defines the contract for managing magazine entities in the bookshop system.
/// </summary>
public interface IMagazineRepository
{
    /// <summary>
    /// Adds a new magazine to the database.
    /// </summary>
    /// <param name="magazine">The magazine to add.</param>
    Task AddMagazineAsync(Magazine magazine);

    /// <summary>
    /// Updates an existing magazine in the database.
    /// </summary>
    /// <param name="magazine">The magazine with updated information.</param>
    Task UpdateMagazineAsync(Magazine magazine);
}