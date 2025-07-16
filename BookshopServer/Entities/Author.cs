namespace BookShopServer.Entities;

/// <summary>
/// Represents an author in the bookshop system, including personal information and associated books.
/// </summary>
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime DOB { get; set; }
    public string? Pseudonym { get; set; }
    public virtual ICollection<Book> Books { get; set; }
}