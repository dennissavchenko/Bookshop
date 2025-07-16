using BookShopServer.Entities;

namespace BookShopServer.Repositories.ItemRepositories.ItemType;

/// <summary>
/// Defines the contract for managing newspaper entities in the bookshop system.
/// </summary>
public interface INewspaperRepository
{
    /// <summary>
    /// Adds a new newspaper to the database.
    /// </summary>
    /// <param name="newspaper">The newspaper to add.</param>
    Task AddNewspaperAsync(Newspaper newspaper);

    /// <summary>
    /// Updates an existing newspaper in the database.
    /// </summary>
    /// <param name="newspaper">The newspaper with updated information.</param>
    Task UpdateNewspaperAsync(Newspaper newspaper);
}