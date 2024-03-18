namespace Application.Bookings.Dtos;
public class BookingDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? RoomId { get; set; }
    public double PricePerDay { get; set; }
    public double DiscountPercentage { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool HasRated { get; set; }
}
