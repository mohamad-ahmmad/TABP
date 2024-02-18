using Domain.Common;

namespace Domain.Entities;
public class CartItem : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = new User();
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = new Room();
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public double Price { get; set; }
}

