using Domain.Common;

namespace Domain.Entities;
public class Room : BaseSoftDeletableAuditableEntity
{
    public string RoomNumber { get; set; } = string.Empty;
    public Guid? RoomInfoId { get; set; }
    public RoomInfo? RoomInfo { get; set; } = null!;
    public int PricePerDay { get; set; }
    public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
