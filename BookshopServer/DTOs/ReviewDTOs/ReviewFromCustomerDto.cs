namespace BookShopServer.DTOs.ReviewDTOs;

/// <summary>
/// Represents the data transfer object for displaying a review from a customer.
/// </summary>
public class ReviewFromCustomerDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
    public DateTime TimeStamp { get; set; }
    public string ItemName { get; set; }
    public int ItemId { get; set; }
    public int CustomerId { get; set; }
}