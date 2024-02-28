namespace Application.CartItems.Dtos;
public class CartItemForCreationDto
{
    public Guid UserId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}
