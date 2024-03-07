using Domain.Common;

namespace Domain.Entities;
public class Booking : BaseEntity
{
    public Guid? UserId { get; set; }
    public User? User { get; set; } = new User();
    public Guid? RoomId { get; set; }
    public Room? Room { get; set; } = new Room();
    public double PricePerDay { get; set; }
    public double DiscountPercentage {  get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool HasRated { get; set; }

}
