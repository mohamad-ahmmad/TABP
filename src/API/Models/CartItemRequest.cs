namespace API.Models;
public class CartItemRequest
{
    /// <summary>
    /// Room id to add to cart
    /// </summary>
    public Guid RoomId { get; set; }
    /// <summary>
    /// Start date of reservation
    /// </summary>
    public DateTime FromDate { get; set; }
    /// <summary>
    /// End date of reservation
    /// </summary>
    public DateTime ToDate { get; set; }
}
